﻿﻿using System;
using ff14bot;
using ff14bot.Managers;
 using LlamaLibrary.Memory;

 namespace LlamaLibrary.Extensions
{
    public static class BagSlotExtentions
    {
        public static bool Split(this BagSlot bagSlot, int amount)
        {
            if (bagSlot.Count > amount)
            {
                lock (Core.Memory.Executor.AssemblyLock)
                {
                    using (Core.Memory.TemporaryCacheState(false))
                    {
                        return Core.Memory.CallInjected64<uint>(Offsets.ItemSplitFunc, new object[4]
                        {
                            Offsets.ItemFuncParam,
                            (uint) bagSlot.BagId,
                            bagSlot.Slot,
                            amount
                        }) == 0;
                    }
                }
            }

            return false;
        }
        
        public static void Discard(this BagSlot bagSlot)
        {
            lock (Core.Memory.Executor.AssemblyLock)
            {
                using (Core.Memory.TemporaryCacheState(false))
                {
                    Core.Memory.CallInjected64<uint>(Offsets.ItemDiscardFunc, new object[3]
                    {
                        Offsets.ItemFuncParam,
                        (uint)bagSlot.BagId,
                        bagSlot.Slot,
                    });
                }
            }
        }
        
        public static bool LowerQuality(this BagSlot bagSlot)
        {
            if (bagSlot.IsHighQuality)
            {
                lock (Core.Memory.Executor.AssemblyLock)
                {
                    using (Core.Memory.TemporaryCacheState(false))
                    {
                        Core.Memory.CallInjected64<uint>(Offsets.ItemLowerQualityFunc, new object[3]
                        {
                            Offsets.ItemFuncParam,
                            (uint) bagSlot.BagId,
                            bagSlot.Slot,
                        });
                    }
                }

                return !bagSlot.IsHighQuality;
            }

            return false;
        }
        
    }
}