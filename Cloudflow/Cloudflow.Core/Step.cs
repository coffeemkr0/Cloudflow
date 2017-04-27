using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudflow.Core
{
    public class Step
    {
        #region Public Methods
        public void Execute(Dictionary<string, object> triggerData)
        {
            //Move the file to another folder
            if (!Directory.Exists("DropFolder"))
            {
                Directory.CreateDirectory("DropFolder");
            }

            var targetFile = Path.Combine("DropFolder", Path.GetFileName(triggerData["FileName"].ToString()));
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }

            File.Copy(triggerData["FileName"].ToString(), targetFile);
        }
        #endregion
    }
}
