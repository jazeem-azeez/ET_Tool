using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace ET_Tool.Common.Logger
{
	public interface IEtLogger
	{
		void Log(string message, EventLevel eventLevel, Exception exception=null);
		
	}
}
