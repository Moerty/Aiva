using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.Internal {
    public static class SimpleChildWindow {
        /// <summary>
        /// Checks if view or datacontext is null
        /// and returns the datacontext
        /// </summary>
        /// <typeparam name="TView"></typeparam>
        /// <typeparam name="TDataContext"></typeparam>
        /// <param name="window"></param>
        /// <param name="context"></param>
        /// <param name="contexxt"></param>
        /// <returns></returns>
        public static Tuple<TView, TDataContext> GetDataContext<TView, TDataContext>(object window, object context) {
            TView view;
            TDataContext dataContext;

            if (!EqualityComparer<TView>.Default.Equals(view = (TView)window, default(TView))) {
                if (!EqualityComparer<TDataContext>.Default.Equals(dataContext = (TDataContext)context, default(TDataContext))) {
                    return new Tuple<TView, TDataContext>(view, dataContext);
                }
            }

            return default(Tuple<TView, TDataContext>);
        }
    }
}
