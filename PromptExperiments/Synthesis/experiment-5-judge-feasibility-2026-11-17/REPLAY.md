# Offline replay

Run from this directory with Node.js 22 and no network.

```powershell
node runner.cjs replay
node evaluate.cjs
node verify.cjs
```

The first command verifies the pre-call freeze and reconstructs the verdict, usage, exact anchors and matrix from saved requests and raw API responses. The second reapplies the frozen thresholds using saved human-style blocker annotations. Review annotations and evaluation code were added after the freeze. Exact anchor matching uses Unicode characters as stored, including punctuation. Invalid calls remain separate in the matrix and fail the all-valid gate. Experiment 4 historically rated time errors as precision issues. The frozen Judge reference treats those three as concrete blockers and keeps the historical material flag for comparison. The final verifier checks all artifact hashes, upstream reference hashes, call ordering and exact evaluation replay.

The requests are self-contained. Rerunning live calls is outside this replay. The existing run lock protects the frozen run from being executed twice.
