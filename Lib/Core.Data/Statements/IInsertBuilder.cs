﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data.Statements {
	public interface IInsertBuilder : INonQueryStatementBuilder, ISingleTableStatement {
	}
}
