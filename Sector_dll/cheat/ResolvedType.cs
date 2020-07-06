using System;

namespace Sector_dll.cheat
{
    class ResolvedType
    {
        public Type Type { get; private set; }

        public string Name { get; }

        public int Diff { get; private set; }

        public readonly ClassSignature signature;

        public ResolvedType(String name, ClassSignature signature)
        {
            Name = name;
            this.signature = signature;
            Diff = int.MaxValue;
        }

        public void Update(ClassSignature sig, Type t)
        {
            if (Diff == 0) return;
            int newDiff = signature.CalculateDifrence(sig);
            if(newDiff < Diff)
            {
                Type = t;
                Diff = newDiff;
            }
        }

        public override string ToString()
        {
            return string.Format("ResolvedType{{Name={0},Type={1},diff={2}}}", Name, Type.Name, Diff);
        }

    }
}
