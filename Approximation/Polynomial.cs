using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib.Data;

namespace MathLib.Approximation {
	/// <summary>
	/// 多項式（３次）近似クラス※未検証クラス
	/// </summary>
	public class Polynomial {
		public double FactorA1 { get; set; }
		public double FactorA2 { get; set; }
		public double FactorA3 { get; set; }
		public double FactorA4 { get; set; }
		public double R2 { get; set; }

		public List<double> HueToGapTable { get; set; }

		public string ConversionMethodText {
			get {
				return string.Format ( "y = {0}x^3{1}x^2{2}x{3}", FactorA4.ToString ( "f30" ), FactorA3.ToString ( "+0.000000000000000000000000000000;-0.000000000000000000000000000000" ),
					FactorA2.ToString ( "+0.000000000000000000000000000000;-0.000000000000000000000000000000" ), FactorA1.ToString ( "+0.000000000000000000000000000000;-0.000000000000000000000000000000" ) );
			}
		}

		public Polynomial() {
			FactorA1 = 0.0;
			FactorA2 = 0.0;
			FactorA3 = 0.0;
			FactorA4 = 0.0;
			HueToGapTable = new List<double> ();
		}

		public void CopyFrom(Polynomial data) {
			FactorA1 = data.FactorA1;
			FactorA2 = data.FactorA2;
			FactorA3 = data.FactorA3;
			FactorA4 = data.FactorA4;
			R2 = data.R2;
			HueToGapTable = data.HueToGapTable;
		}

		public static void ExecuteLinest ( double[] x, double[] y ) {
		}

		/// <summary>
		/// 最小２乗法による近似３次多項式取得
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public static Polynomial ExecuteLSM(double[] x, double[] y) {
			int lenY = y.Count();
			int lenX = x.Count();
			Polynomial tbl = new Polynomial();
			try {
				if ( lenX == lenY ) {
					Matrix matY = new Matrix ( 4, 1 );
					Matrix matX = new Matrix ( 4, 4 );

					List<double> sigmaXList = new List<double> ();
					List<double> sigmaYList = new List<double> ();

					sigmaXList.Add ( x.Sum () );

					for ( int i = 2; i <= 6; i++ ) {
						double sum = 0.0;
						for ( int j = 0; j < lenX; j++ ) {
							sum += Math.Pow ( x[j], i );
						}
						sigmaXList.Add ( sum );
					}

					sigmaYList.Add ( y.Sum () );
					for ( int i = 1; i <= 3; i++ ) {
						double sum = 0.0;
						for ( int j = 0; j < lenY; j++ ) {
							if ( i == 1 ) {
								sum += x[j] * y[j];
							} else {
								sum += Math.Pow ( x[j], i ) * y[j];
							}
						}
						sigmaYList.Add ( sum );
					}

					matY[0, 0] = sigmaYList[0];
					matY[1, 0] = sigmaYList[1];
					matY[2, 0] = sigmaYList[2];
					matY[3, 0] = sigmaYList[3];

					matX[0, 0] = lenX; matX[0, 1] = sigmaXList[0]; matX[0, 2] = sigmaXList[1]; matX[0, 3] = sigmaXList[2];

					matX[1, 0] = sigmaXList[0]; matX[1, 1] = sigmaXList[1]; matX[1, 2] = sigmaXList[2]; matX[1, 3] = sigmaXList[3];

					matX[2, 0] = sigmaXList[1]; matX[2, 1] = sigmaXList[2]; matX[2, 2] = sigmaXList[3]; matX[2, 3] = sigmaXList[4];

					matX[3, 0] = sigmaXList[2]; matX[3, 1] = sigmaXList[3]; matX[3, 2] = sigmaXList[4]; matX[3, 3] = sigmaXList[5];

					Matrix invX = matX.InverseRowReduction ();

					Matrix ans = invX * matY;

					tbl.FactorA1 = ans[0, 0];
					tbl.FactorA2 = ans[1, 0];
					tbl.FactorA3 = ans[2, 0];
					tbl.FactorA4 = ans[3, 0];

					tbl.R2 = ExecuteR2 ( x, y, tbl );
				}
			} catch {
			}

			return tbl;
		}

		private static double ExecuteR2(double[] x, double[] y, Polynomial m) {
			List<double> listX = new List<double> ();
			List<double> listY = new List<double> ();

			listY.AddRange ( y );
			listX.AddRange ( x );

			double mx = x.Average ();
			double my = y.Average ();

			double sr = 0.0, se = 0.0, sy = 0.0;
			System.Threading.Tasks.Parallel.Invoke ( new Action[]{
                ()=>{
                    for ( int i = 0; i < listX.Count; i++ ) {
                        sr += Math.Pow( ( y[i] - my ), 2 );
                    }
                },
                ()=>{
                    for ( int i = 0; i < listX.Count; i++ ) {
						se += Math.Pow ( y[i] - ( m.FactorA4 * Math.Pow ( x[i], 3 ) + m.FactorA3 * Math.Pow ( x[i], 2 ) + m.FactorA2 * x[i] + m.FactorA1), 2 );
                    }
                }
            } );

			sy = sr + se;

			double r = sr / sy;

			return r;
		}
	}
}
