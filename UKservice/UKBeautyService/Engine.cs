using UKBeautyLibrary.BLCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OmnimarkAmazon.Service
{
    public class Engine
    {
        public void Process()
        {
            try
            {
                UKItemSearch.UpdateAsin();
                LogErrors.WriteCustomErrorToFile("Service Started At" + DateTime.Now.ToShortTimeString());
            }
            catch (Exception ex)
            {
                LogErrors.WriteErrorToFile(ex);
            }
        }
    }
}
