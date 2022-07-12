// -----------------------------------
// 
// 模块说明：IOpener.cs
// 
// 创建人员：AE
// 创建日期：2022-04-14
// -----------------------------------

using AEAssist.AI;
using System;
using System.Collections.Generic;

namespace AEAssist.Opener
{
    public interface IOpener
    {
        int Check();
        List<Action<SpellQueueSlot>> Openers { get; }
    }
}