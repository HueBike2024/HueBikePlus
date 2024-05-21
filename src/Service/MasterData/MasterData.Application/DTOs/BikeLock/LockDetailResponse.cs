﻿using Core.SeedWork.ExtendEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterData.Application.DTOs.BikeLock
{
    public class LockDetailResponse : BaseExtendEntities
    {
        public long Id { get; set; }
        public string LockName { get; set; }
        public string PathQr { get; set; }
        public byte[] QrCodeImage { get; set; }
        public int Power { get; set; }
        public bool IsUsed { get; set; }
    }
}
