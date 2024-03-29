﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProgrammersBlog.Entities.Dtos;

namespace ProgrammersBlog.Mvc.Models
{
    public class CommentAddAjaxViewModel
    {
        public CommentAddDto ComentAddDto { get; set; }
        public string CommentAddPartial { get; set; }
        public CommentDto CommentDto { get; set; }
    }
}
