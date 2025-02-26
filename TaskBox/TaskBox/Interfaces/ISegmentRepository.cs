﻿using TaskBox.Shared.Models;

namespace TaskBox.Interfaces
{
	public interface ISegmentRepository
	{
		public Segment GetSegmentFromId(int SegmentId);
		public List<Segment> GetSegmentsFromProjectId(int ProjectId);
		public ApiResponse CreateSegment(Segment segment, int UserId);
		public ApiResponse UpdateSegment(int UserId, Segment Segment);
		public ProjectUserPermission UserSegmentPermission(int SegmentId, int UserId);
	}
}
