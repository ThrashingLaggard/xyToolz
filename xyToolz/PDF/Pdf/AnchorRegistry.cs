namespace xyToolz.Pdf
{
using System;
using System.Collections.Generic;
using PdfSharpCore.Pdf;

    /// <summary>
    /// Stores navigation anchors (type headings) and their exact PDF landing positions.
    /// </summary>
    public sealed class AnchorRegistry
    {
        private readonly Dictionary<string, AnchorTarget> _map =
            new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Registers an anchor for a canonical key (e.g. "A.B.Outer.Inner" or "Global (Default).Program").
        /// </summary>
        public void Register(string canonicalKey, PdfPage page, double yOnPage)
            => _map[canonicalKey] = new AnchorTarget(page, yOnPage);

        /// <summary>
        /// Tries to get the target location for a canonical key.
        /// </summary>
        public bool TryFind(string canonicalKey, out AnchorTarget target)
            => _map.TryGetValue(canonicalKey, out target);

        /// <summary>
        /// Enumerates all registered keys (useful for fuzzy/bare-name matching).
        /// </summary>
        public IEnumerable<string> Keys => _map.Keys;
    }
}
