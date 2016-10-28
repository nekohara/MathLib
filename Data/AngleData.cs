using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Data {
	/// <summary>
	/// 角度
	/// </summary>
	public class AngleData {
		double _degree;
		double _radian;
		public double Degree {
			get{
				return _degree;
			}
			set{
				_degree = value;
				_radian = ToRadian(_degree);
			}
		}
		public double Radian {
			get {
				return _radian;
			}
			set {
				_radian = value;
				_degree = ToDegree(_radian);
			}
		}

		/// <summary>
		/// Radian変換
		/// </summary>
		/// <param name="degree">Degree角度</param>
		/// <returns></returns>
		public static double ToRadian(double degree){
			return degree * Math.PI / 180.0;
		}

		/// <summary>
		/// Degree変換
		/// </summary>
		/// <param name="radian">Radian角度</param>
		/// <returns></returns>
		public static double ToDegree(double radian) {
			return radian * 180.0 / Math.PI;
		}


		public static AngleData CreateAngleDegree(double degree){
			AngleData angle = new AngleData();
			angle.Degree = degree;
			return angle;
		}

		public static AngleData CreateAngleRadian(double radian) {
			AngleData angle = new AngleData();
			angle.Radian = radian;
			return angle;
		}
	}
}
