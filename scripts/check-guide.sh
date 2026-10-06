#!/usr/bin/env bash
# Checks the guide before a commit:
#   1. every table-of-contents link (#anchor) points to a real heading, using GitHub's slug rules;
#   2. writes artifacts/mermaid-check.html, a page that parses every Mermaid diagram with the real
#      Mermaid library. Open it in a browser: it ends with "All N diagrams parsed." or lists failures.
# Mermaid needs a browser to parse, so step 2 cannot run in the terminal. Uses node (no packages).
# Usage: scripts/check-guide.sh [path/to/guide.md]   (default: docs/DESIGN_PATTERNS_GUIDE.md)
set -euo pipefail

guide="${1:-docs/DESIGN_PATTERNS_GUIDE.md}"
root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
out_dir="$root/artifacts"
mkdir -p "$out_dir"

node - "$guide" "$out_dir/mermaid-check.html" <<'EOF'
const fs = require('fs');
const [guidePath, htmlPath] = process.argv.slice(2);
const lines = fs.readFileSync(guidePath, 'utf8').split(/\r?\n/);

// GitHub's slug: lower-case, drop everything except letters, digits, spaces, '-' and '_',
// spaces become '-'. Repeated headings get -1, -2... Inline code backticks are dropped too.
const slugCounts = new Map();
function slug(text) {
  const base = text.trim().toLowerCase()
    .replace(/[^\p{L}\p{N} _-]/gu, '')
    .replace(/ /g, '-');
  const n = slugCounts.get(base) ?? 0;
  slugCounts.set(base, n + 1);
  return n === 0 ? base : `${base}-${n}`;
}

const anchors = new Set();
const links = [];
const diagrams = [];
let fence = null;          // the fence that opened the current code block ("```" or "~~~")
let current = null;        // the Mermaid block being collected

lines.forEach((line, i) => {
  const fenceMatch = line.match(/^\s*(```+|~~~+)\s*(\S*)/);
  if (fenceMatch) {
    if (fence === null) {
      fence = fenceMatch[1];
      if (fenceMatch[2] === 'mermaid') current = { line: i + 1, text: [] };
      return;
    }
    if (line.trim().startsWith(fence)) {
      if (current) diagrams.push({ line: current.line, text: current.text.join('\n') });
      fence = null; current = null;
      return;
    }
  }
  if (fence !== null) { if (current) current.text.push(line); return; }

  const heading = line.match(/^#{1,6}\s+(.*?)\s*#*\s*$/);
  if (heading) anchors.add(slug(heading[1]));
  for (const m of line.matchAll(/\]\(#([^)\s]+)\)/g)) links.push({ line: i + 1, anchor: m[1] });
});

let missing = 0;
for (const l of links) {
  if (!anchors.has(decodeURIComponent(l.anchor))) {
    console.log(`MISSING ANCHOR #${l.anchor} (line ${l.line})`);
    missing++;
  }
}
console.log(`Anchors: ${links.length - missing} of ${links.length} links resolve.`);

const html = `<!doctype html>
<html><head><meta charset="utf-8"><title>Mermaid check</title></head>
<body><pre id="out">Parsing...</pre>
<script src="https://cdn.jsdelivr.net/npm/mermaid@11/dist/mermaid.min.js"></script>
<script>
const diagrams = ${JSON.stringify(diagrams)};
(async () => {
  mermaid.initialize({ startOnLoad: false });
  const out = [];
  let failed = 0;
  for (const d of diagrams) {
    try { await mermaid.parse(d.text); out.push('OK line ' + d.line); }
    catch (e) { failed++; out.push('FAILED line ' + d.line + ': ' + String(e.message || e).split('\\n')[0]); }
  }
  out.push(failed === 0 ? 'All ' + diagrams.length + ' diagrams parsed.'
                        : failed + ' of ' + diagrams.length + ' diagrams failed.');
  document.getElementById('out').textContent = out.join('\\n');
})();
</script></body></html>`;
fs.writeFileSync(htmlPath, html);
console.log(`Mermaid: ${diagrams.length} diagrams written to ${htmlPath} (open it in a browser).`);
process.exit(missing === 0 ? 0 : 1);
EOF
