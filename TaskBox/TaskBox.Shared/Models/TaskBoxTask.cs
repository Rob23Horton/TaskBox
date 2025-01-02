﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseConnection.Attributes;

namespace TaskBox.Shared.Models
{
	public class TaskBoxTask : GanttChartObject
	{
		[InsertIgnore]
		public override int Id { get; set; }
		public int SegmentCode { get; set; }
	}
}