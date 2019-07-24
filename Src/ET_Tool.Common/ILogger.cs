using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace ET_Tool.Common
{
	public interface ILogger
	{
		void Log(string message, Exception ex, EventLevel eventLevel);
		
	}
}
