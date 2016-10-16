using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Data {
	/// <summary>
	/// 座標データ
	/// </summary>
	public struct PointD {
		public double X;
		public double Y;
		public PointD(double x, double y){
			X = x;
			Y = y;
		}
	}
}
