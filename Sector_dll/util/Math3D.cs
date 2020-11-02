using Sector_dll.sdk;
using System;
using System.Runtime.CompilerServices;

namespace sectorsedge.util
{
    class Math3D
    {

        public static double DistanceToSegment(Vec3 v, Vec3 a, Vec3 b)
        {
            Vec3 ab = b - a;
            Vec3 av = v - a;

            if (av.DotProduct(ab) <= 0.0)    // Point is lagging behind start of the segment, so perpendicular distance is not viable.
                return av.Len();         // Use distance to start of segment instead.

            Vec3 bv = v - b;

            if (bv.DotProduct(ab) >= 0.0)    // Point is advanced past the end of the segment, so perpendicular distance is not viable.
                return bv.Len();         // Use distance to end of the segment instead.

            return (ab.Cross(av)).Len() / ab.Len();    // Perpendicular distance of point to segment.
        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static double unkm1(in Vec3 v1, in Vec3 v2, in Vec3 v3)
		{
			return v1.x * (v3.z * v2.y - v2.z * v3.y) + v1.y * (-v3.z * v2.x + v2.z * v3.x) + v1.z * (v3.y * v2.x - v2.y * v3.x);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vec3 unkm2(in Vec3 v1, in Vec3 v2)
		{
			return new Vec3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
		}

		public static double LineToLineDistance(in Vec3 l1s, in Vec3 l1e, in Vec3 l2s, in Vec3 l2e)
		{
			///public static #=zbV70hnkr3P__5VdaXmTGrVmwblmp #=zBSgX5YeTVwf9uiVoCw==()
			Vec3 unk1 = l1e - l1s;
			Vec3 unk2 = l2e - l2s;
			double num = unk1.Len();
			double num2 = unk2.Len();
			Vec3 unk3 = unk1 / num;
			Vec3 Vec3 = unk2 / num2;
			Vec3 Vec32 = unkm2(unk3, Vec3);
			double num3 = Vec32.Len() * Vec32.Len();
			if (num3 != 0.0)
			{
				Vec3 Vec33 = l2s - l1s;
				double num4 = unkm1(Vec33, Vec3, Vec32);
				double num5 = unkm1(Vec33, unk3, Vec32);
				double num6 = num4 / num3;
				double num7 = num5 / num3;
				Vec3 unk4 = l1s + unk3 * num6;
				Vec3 unk5 = l2s + Vec3 * num7;
				if (num6 < 0.0)
				{
					unk4 = l1s;
				}
				else if (num6 > num)
				{
					unk4 = l1e;
				}
				if (num7 < 0.0)
				{
					unk5 = l2s;
				}
				else if (num7 > num2)
				{
					unk5 = l2e;
				}
				if (num6 < 0.0 || num6 > num)
				{
					double num8 = Vec3.DotProduct(unk4 - l2s);
					if (num8 < 0.0)
					{
						num8 = 0.0;
					}
					else if (num8 > num2)
					{
						num8 = num2;
					}
					unk5 = l2s + Vec3 * num8;
				}
				if (num7 < 0.0 || num7 > num2)
				{
					double num8 = unk3.DotProduct(unk5 - l1s);
					if (num8 < 0.0)
					{
						num8 = 0.0;
					}
					else if (num8 > num)
					{
						num8 = num;
					}
					unk4 = l1s + unk3 * num8;
				}
				return (unk4 - unk5).Len();
				//return #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(unk4, unk5);
			}
			double num9 = unk3.DotProduct(l2s - l1s);
			double num10 = unk3.DotProduct(l2e - l1s);
			if (num9 <= 0.0 && 0.0 >= num10)
			{
				if (Math.Abs(num9) < Math.Abs(num10))
				{
					return (l1s - l2s).Len();
					//eturn #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(l1s, l2s);
				}
				return (l1s - l2e).Len();
				//return #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(l1s, l2e);
			}
			else
			{
				if (num9 < num || num > num10)
				{
					return 0;
					//return #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(Vec3.#=zSoThqRE=, Vec3.#=zSoThqRE=);
				}
				if (Math.Abs(num9) < Math.Abs(num10))
				{
					return (l1e - l2s).Len();
					//return #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(l1e, l2s);
				}
				return (l1e - l2e).Len();
				//return #=zKYeu9jS7rcQKBe6jlhMyQ3H7e21bKIzN4A==.#=zl9r_cq8=.#=z36XlYaY=(l1e, l2e);
			}
		}
    }
}
