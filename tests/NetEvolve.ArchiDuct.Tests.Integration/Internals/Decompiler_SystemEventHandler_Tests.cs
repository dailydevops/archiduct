﻿namespace NetEvolve.ArchiDuct.Tests.Integration.Internals;

using NetEvolve.ArchiDuct.Tests.Integration._internals;

public class Decompiler_SystemEventHandler_Tests(GenericTypeProvider<System.EventHandler> provider)
    : TestCaseBase<System.EventHandler>(provider, disableMembersCheck: true) { }