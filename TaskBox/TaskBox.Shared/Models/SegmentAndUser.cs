using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskBox.Shared.Models
{
	public class SegmentAndUser
	{
		public SegmentAndUser()
		{
			this.Segment = new Segment();
		}

		public SegmentAndUser(int UserId, Segment Segment)
		{
			this.UserId = UserId;
			this.Segment = Segment;
		}


		public int UserId { get; set; }
		public Segment Segment { get; set; }
	}
}
