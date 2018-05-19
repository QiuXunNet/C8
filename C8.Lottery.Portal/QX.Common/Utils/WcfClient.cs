
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace QX.Common
{
    public class WcfClient
    {
        public static TReturn UseService<TChannel, TReturn>(Func<TChannel, TReturn> func)
        {
            var chanFactory = new ChannelFactory<TChannel>("*");
            TChannel channel = chanFactory.CreateChannel();
            ((IClientChannel)channel).Open();
            TReturn result = func(channel);
            try
            {
                ((IClientChannel)channel).Close();
            }
            catch
            {
                ((IClientChannel)channel).Abort();
            }
            return result;
        }
        public static void UseService<TChannel>(Action<TChannel> action)
        {
            var chanFactory = new ChannelFactory<TChannel>("*");
            TChannel channel = chanFactory.CreateChannel();
            ((IClientChannel)channel).Open();
            action(channel);
            try
            {
                ((IClientChannel)channel).Close();
            }
            catch
            {
                ((IClientChannel)channel).Abort();
            }
        }
    }
}
