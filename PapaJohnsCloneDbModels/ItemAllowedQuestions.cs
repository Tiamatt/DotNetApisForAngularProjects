﻿using System;
using System.Collections.Generic;

namespace DotNetApisForAngularProjects.PapaJohnsCloneDbModels
{
    public partial class ItemAllowedQuestions
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int QuestionId { get; set; }

        public Item Item { get; set; }
        public Question Question { get; set; }
    }
}
