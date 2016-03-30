using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    public interface ISpawner
    {
        void SpawnSmallWalker();

        void SpawnBigWalker();

        void Wait(double time);

        void Debug(string message);
    }
}
