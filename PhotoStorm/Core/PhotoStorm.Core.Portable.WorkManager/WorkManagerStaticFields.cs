using System.Collections.Generic;
using PhotoStorm.Core.Portable.Works.Works;

namespace PhotoStorm.Core.Portable.WorkManager
{
    public static class WorkManagerStaticFields
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
        public static HashSet<IWork> Works = new HashSet<IWork>();
    }
}
