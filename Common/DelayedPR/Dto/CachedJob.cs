using System;

namespace Common.DelayedPR.Dto
{
    internal class CachedJob
    {
        public string AssemblyName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime RunAt { get; set; }
        public MethodParameter[] MethodParameters { get; set; }
    }
}
