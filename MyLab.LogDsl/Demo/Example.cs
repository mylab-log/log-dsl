using System;
using Microsoft.Extensions.Logging;
using MyLab;
using MyLab.LogDsl;

namespace Demo
{
    class Example
    {
        private readonly DslLogger _log;

        public Example(ILogger<Example> logger)
        {
            _log = logger.Dsl();
        }

        public void Example1_SimpleAct()
        {
            _log.Act("I did it").Write();
        }

        public void Example2_SimpleDebug()
        {
            _log.Debug("I did debug").Write();
        }

        public void Example3_SimpleError()
        {
            _log.Error("Something wrong").Write();
        }

        public void Example4_ErrorWithException()
        {
            Exception exception;
            try
            {
                throw new NullReferenceException();
            }
            catch (Exception e)
            {
                exception = e;
            }

            _log.Error(exception).Write();
        }

        public void Example5_WithConditions()
        {
            int debugParameter1 = 1;
            int debugParameter2 = 10;

            _log.Debug("I did debug")
                .AndFactIs("I'm tired")
                .AndFactIs("Debug password", "very secret password")
                .AndFactIs(() => debugParameter1 > 5)
                .AndFactIs(() => debugParameter2 > 5)
                .Write();
        }

        public void Example6_WithMarkers()
        {
            _log.Act("I did it")
                .AndMarkAs("important")
                .Write();
        }

        public void Example7_WithExceptionParameters()
        {
            TopLevelActon();

            void BottomLevelAction()
            {
                throw new Exception("SQL server error")
                    .AndMarkAs("db-error");
            }

            void MiddleLevelAction()
            {
                try
                {
                    BottomLevelAction();
                }
                catch (Exception e)
                {
                    e.AndFactIs("userId", 100)
                     .AndMarkAs("vip-client");
                    throw;
                }
            }

            void TopLevelActon()
            {
                try
                {
                    MiddleLevelAction();
                }
                catch (Exception e)
                {
                    _log.Error(e)
                        .AndFactIs("ip", "90.109.220.01")
                        .Write();
                }
            }
        }
    }
}