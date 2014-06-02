﻿using System;

namespace Aps.DomainBase
{
    public static class DateExtensions
    {
        public static bool Between(this DateTime input, DateTime date1, DateTime date2)
        {
            return (input > date1 && input < date2);
        }
    }
}