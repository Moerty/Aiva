using System.Collections.Generic;

namespace Aiva.Gui.Internal {
    public static class ChildWindow {
        public static T GetDatacontext<T>(object datacontext) {
            T convertedDataContext;
            if (!EqualityComparer<T>.Default.Equals(convertedDataContext = (T)datacontext, default(T))) {
                return convertedDataContext;
            } else {
                return default(T);
            }
        }
    }
}