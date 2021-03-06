﻿using System.Collections.Generic;

namespace BlobService.Core.Models
{
    public class BlobViewModel
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string DownloadRelativeUrl { get; set; }
        public IEnumerable<BlobMetaDataViewModel> MetaData { get; set; }
    }
}
