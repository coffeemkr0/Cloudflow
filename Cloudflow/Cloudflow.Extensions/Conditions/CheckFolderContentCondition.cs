using Cloudflow.Core.Extensions;
using Cloudflow.Core.Extensions.ExtensionAttributes;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Cloudflow.Extensions.Conditions
{
   public class CheckFolderContentCondition
    {
        readonly CheckFolderContentConditionConfiguration _configuration;

        public CheckFolderContentCondition(CheckFolderContentConditionConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckCondition()
        {
            foreach (var fileNameMask in _configuration.FileNameMasks)
            {
                if (Directory.GetFiles(_configuration.Folder, fileNameMask, SearchOption.TopDirectoryOnly).Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
