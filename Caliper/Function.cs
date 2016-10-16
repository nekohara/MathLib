using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib.Data;

namespace MathLib.Caliper {
	public class Function {
	

		/// <summary>
		/// 内積(２次元ベクトル)
		/// </summary>
		/// <param name="v1">ベクトル１</param>
		/// <param name="v2">ベクトル２</param>
		/// <returns></returns>
		public static double Dot(PointD v1, PointD v2) {
			return v1.X * v2.X + v1.Y + v2.Y;
		}

		/// <summary>
		/// 重心計算
		/// </summary>
		/// <param name="plist">対象点群</param>
		/// <returns></returns>
		public static PointD CalculateCenter(List<PointD> plist) {
			PointD center = new PointD();

			center.X = plist.Sum(p => p.X) / plist.Count;
			center.Y = plist.Sum(p => p.Y) / plist.Count;

			return center;
		}

		/// <summary>
		/// 直線と垂直の関係となる指定点を通る直線の交点を求める
		/// </summary>
		/// <param name="line"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static PointD VerticalPoint(Line line, PointD point){
			PointD res = new PointD();

			if (line.A != 0 && line.B == 0) {
				res.Y = point.Y;
				res.X = -1 * line.C / line.A;
			} else if (line.A == 0 && line.B != 0) {
				res.X = point.X;
				res.Y = -1 * line.C / line.B;
			} else {
				double a = -1 * (line.A / line.B);
				double b = -1 * (line.C / line.B);
				double px = point.X;
				double py = point.Y;

				res.X = (a * (point.Y - b) + point.X) / (Math.Pow(a, 2) + 1);
				res.Y = a * (a * (point.Y - b) + point.X) / (Math.Pow(a, 2) + 1) + b;
			}

			return res;
		}

		/// <summary>
		/// 直線と垂直の関係となる指定点を通る直線を求める
		/// </summary>
		/// <param name="line"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static Line VerticalLine(Line line, PointD point) {
			Line result = line;

			if ( line.A != 0 && line.B == 0 ) {
				result = new Line(new PointD(0, point.Y), point);
			} else if ( line.A == 0 && line.B != 0 ) {
				result = new Line(new PointD(point.X, 0), point);
			} else {
				double a = -1 * ( line.A / line.B );
				double b = -1 * ( line.C / line.B );
				double px = point.X;
				double py = point.Y;

				PointD point2 = new PointD();

				point2.X = ( a * ( point.Y - b ) + point.X ) / ( Math.Pow(a, 2) + 1 );
				point2.Y = a * ( a * ( point.Y - b ) + point.X ) / ( Math.Pow(a, 2) + 1 ) + b;

				result = new Line(point, point2);
			}

			return result;
		}

		/// <summary>
		/// 三角形の内心
		/// </summary>
		/// <param name="triangle">三角形</param>
		/// <returns></returns>
		public static PointD TraiangleInnerCenter(List<PointD> triangle) {
			PointD innerCenter = new PointD();

			PointD A = triangle[0];
			PointD B = triangle[1];
			PointD C = triangle[2];

			double a = Math.Sqrt(Math.Pow(B.X - C.X, 2) + Math.Pow(B.Y - C.Y, 2));
			double b = Math.Sqrt(Math.Pow(A.X - C.X, 2) + Math.Pow(A.Y - C.Y, 2));
			double c = Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));

			innerCenter.X = ( a * A.X + b * B.X + c * C.X ) / ( a + b + c );
			innerCenter.Y = ( a * A.Y + b * B.Y + c * C.Y ) / ( a + b + c );

			return innerCenter;
		}

		/// <summary>
		/// 頂点シフト（三角ポリゴン使用）
		/// </summary>
		/// <param name="triangle">三角形頂点</param>
		/// <param name="center">全体図形の中心</param>
		/// <param name="shift">シフト量</param>
		/// <returns></returns>
		public static PointD CalculateShiftPoint(List<PointD> triangle, PointD center, double shift) {
			PointD shiftPoint = new PointD();
			PointD target = triangle[1];
			///３角形重心計算
			PointD tCenter = CalculateCenter(triangle);

			///３角形内心計算
			PointD iCenter = TraiangleInnerCenter(triangle);

			Line tLine = new Line(triangle[0], triangle[2]);
			//Caliper.Line vLine = Caliper.VerticalLine(tLine, target);
			Line vLine = new Line(iCenter, target);


			PointD vector1 = new PointD(tCenter.X - target.X, tCenter.Y - target.Y);
			PointD vector2 = new PointD(target.X - center.X, target.Y - center.Y);

			vector1.X = vector1.X != 0 ? vector1.X / Math.Abs(vector1.X) : 0;
			vector1.Y = vector1.Y != 0 ? vector1.Y / Math.Abs(vector1.Y) : 0;
			vector2.X = vector2.X != 0 ? vector2.X / Math.Abs(vector2.X) : 0;
			vector2.Y = vector2.Y != 0 ? vector2.Y / Math.Abs(vector2.Y) : 0;

			//重心が外向きか内向きか判定
			double dot = Dot(vector1, vector2);

			if ( dot < 0 ) {	//内向きの場合は逆変換で外向きに変換
				vector1.X *= -1;
				vector1.Y *= -1;
				//vector2.X *= -1;
				//vector2.Y *= -1;
			}

			if ( vector1.X != 0 || vector1.Y != 0 ) {
				//頂点シフト
				shiftPoint = vLine.CalculateShiftPoint(target, shift, vector1);
			} else {
				//頂点シフト
				shiftPoint = vLine.CalculateShiftPoint(target, shift, vector2);
			}

			return shiftPoint;
		}
	}
}
