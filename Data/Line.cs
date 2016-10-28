using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Data {
	/// <summary>
	/// 直線クラス
	/// </summary>
	public class Line {
		public double A;
		public double B;
		public double C;
		public Line(double a, double b, double c){
			A = a;
			B = b;
			C = c;
		}

		public Line(PointD p1, PointD p2) {
			A = p1.Y - p2.Y;
			B = -1 * ( p1.X - p2.X );
			C = p1.X * p2.Y - p2.X * p1.Y;
		}

		/// <summary>
		/// 直線上のにある点を指定距離分移動
		/// </summary>
		/// <param name="point">対象点</param>
		/// <param name="shift">移動量</param>
		/// <param name="direction">移動方向</param>
		/// <returns>移動座標</returns>
		public PointD CalculateShiftPoint(PointD point, double shift, PointD direction) {
			PointD p = point;

			if ( A != 0 && B == 0 ) {

				p.Y = p.Y + ( shift * direction.Y );
			} else if ( A == 0 && B != 0 ) {

				p.X = p.X + ( shift * direction.X );
			} else {
				double a = -1 * ( A / B );
				double b = -1 * ( C / B );
				double px = point.X;
				double py = point.Y;
				p.X = px + ( ( direction.X * shift ) / Math.Sqrt(Math.Pow(a, 2) + 1.0) );
				p.Y = ( a * p.X ) + b;
			}
			return p;
		}

		/// <summary>
		/// 傾き取得（y=ax+b）
		/// </summary>
		/// <returns></returns>
		public double GetSlopeA(){
			double slope = 0;

			if(A != 0 && B!=0){
				slope = -1 * ( A / B );
			}

			return slope;
		}

		/// <summary>
		/// 切片取得（y=ax+b）
		/// </summary>
		/// <returns></returns>
		public double GetInterceptB(){
			double intercept = 0;

			if ( A != 0 && B != 0 ) {
				intercept = -1 * ( C / B );
			}

			return intercept;
		}
	}
}
