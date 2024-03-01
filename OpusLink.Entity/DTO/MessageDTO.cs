﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpusLink.Entity.DTO
{
	public class MessageDTO
	{
        public int MessageID { get; set; }
        public int ChatBoxID { get; set; }
        public bool FromEmployer { get; set; }
        public DateTime DateCreated { get; set; }
        public string MessageContent { get; set; }
        public string? EmployerName { get; set; }
        public string? FreelancerName { get; set; }
    }
}
