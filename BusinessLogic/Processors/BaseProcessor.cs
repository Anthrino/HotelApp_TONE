using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Processors
{
	public class BaseProcessor
	{
		public UserProcessor userProcessor;
		public ItemProcessor itemProcessor;
		public CartProcessor cartProcessor;
		public OrderProcessor orderProcessor;
	}
}
