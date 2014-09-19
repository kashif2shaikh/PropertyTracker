// IMvxImageCache.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com
//
// Modified By Kashif: Added ability to purge images from memory, so image can be refreshed again.
// This just modifies MvxImageCache in Core plugin, so should work across iOS/Android without issue.

using System;

namespace Cirrious.MvvmCross.Plugins.DownloadCache
{
    public interface IMvxImageCache<T>
    {
        void RequestImage(string url, Action<T> success, Action<Exception> error);

		// New method.
		void PurgeImage(string url);
    }
}