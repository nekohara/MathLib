using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Approximation {
	/// <summary>
	/// 一次方程式近似クラス
	/// </summary>
	public class Linear {
		  private double _formulaA;
        private double _interceptB;
        private double _formulaR2;

        /// <summary>
        /// 傾きＡ
        /// </summary>
        public double FormulaA {
            get {
                return _formulaA;
            }
            set {
                _formulaA = value;
            }
        }

        /// <summary>
        /// 切片Ｂ
        /// </summary>
        public double InterceptB {
            get {
                return _interceptB;
            }
            set {
                _interceptB = value;
            }
        }

        /// <summary>
        /// 積率相関係数
        /// </summary>
        public double FormulaR2 {
            get {
                return _formulaR2;
            }
            set {
                _formulaR2 = value;
            }
        }

		public Linear() {
            _formulaA = 1.0;
            _interceptB = 0.0;
            _formulaR2 = 1.0;
        }

		public void CopyFrom(Linear param) {
            _formulaA = param._formulaA;
            _interceptB = param._interceptB;
            _formulaR2 = param._formulaR2;
        }

        /// <summary>
        /// 最小2乗による近似直線算出(y=ax)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		public static Linear CalculateA(List<double> x, List<double> y) {
			Linear result = new Linear();

            if ( x.Count != y.Count ) {
                return null;
            }

            double xy = 0.0;
            double x2 = 0.0;

            //偏差積和、偏差平方和
            System.Threading.Tasks.Parallel.Invoke( new Action[]{
                ()=>{
                    for ( int i = 0; i < x.Count; i++ ) {
                        xy += x[i] * y[i];
                    }
                },
                ()=>{
                    x.ForEach(xi => {
                        x2 += Math.Pow( xi, 2 );
                    });
                }
            } );

            result._formulaA = Math.Round( xy / x2, 4, MidpointRounding.AwayFromZero );
            result._interceptB = 0.0;

            double mx = x.Average();
            double my = y.Average();

            double sr = 0.0, se=0.0, sy = 0.0;
            System.Threading.Tasks.Parallel.Invoke( new Action[]{
                ()=>{
                    for ( int i = 0; i < x.Count; i++ ) {
                        sr += Math.Pow( ( y[i] - my ), 2 );
                    }
                },
                ()=>{
                    for ( int i = 0; i < x.Count; i++ ) {
                        se += Math.Pow( y[i] - ( result._formulaA * x[i] ), 2 );
                    }
                }
            } );

            sy = sr + se;

            double r = sr / sy;
            result._formulaR2 = Math.Round( r, 6, MidpointRounding.AwayFromZero );
            return result;
        }

        /// <summary>
        /// 最小2乗による近似直線算出
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		public static Linear Calculate(List<double> x, List<double> y) {
			Linear result = new Linear();

            if (x.Count != y.Count) {
                return null;
            }

            double mx = x.Average();
            double my = y.Average();

            double sigmaX = 0.0, sigmaY = 0.0, sigmaXY = 0.0;
            double varX = 0.0, varY = 0.0;

            //偏差積和、偏差平方和
            System.Threading.Tasks.Parallel.Invoke(new Action[]{
                ()=>{
                    x.ForEach(xi => {
                        double div_x = xi - mx;
                        sigmaX += div_x;
                        varX += Math.Pow(div_x, 2);
                    });
                },
                ()=>{
                    y.ForEach(yi => {
                        double div_y = yi - my;
                        sigmaY += div_y;
                        varY += Math.Pow(div_y, 2);
                    });
                },
                ()=>{
                    for(int i=0; i < x.Count; i++) {
                        sigmaXY += ( x[i] - mx ) * ( y[i] - my );
                    }
                }
            });

            result._formulaA = Math.Round(sigmaXY / varX, 6, MidpointRounding.AwayFromZero);
            result._interceptB = Math.Round( my - ( mx * result._formulaA ), 6, MidpointRounding.AwayFromZero );

            double r = sigmaXY / Math.Sqrt( varX * varY );
            result._formulaR2 = Math.Round( Math.Pow( r, 2 ), 6, MidpointRounding.AwayFromZero );
            
            return result;
        }

        public string toStringFormula ( ) {
            return string.Format("Y={0:0.000000}X", _formulaA);
        }

        public string toStringFormulaB ( ) {
            string sign = _interceptB > 0 ? "+" : "-";
            return string.Format( "Y={0:0.000000}X{1}{2:0.000000}", _formulaA, sign, Math.Abs( _interceptB ) );
        }

        public string toStringR2 ( ) {
            return string.Format( "R^2={0:0.000000}", _formulaR2 );
        }

        public string toShortStringFormula ( ) {
            return string.Format( "Y={0:0.000}X", _formulaA );
        }

        public string toShortStringFormulaB ( ) {
            string sign = _interceptB > 0 ? "+" : "-";
            return string.Format( "Y={0:0.000}X{1}{2:0.000}", _formulaA, sign, Math.Abs( _interceptB ) );
        }

        public string toShortStringR2 ( ) {
            return string.Format( "R^2={0:0.000}", _formulaR2 );
        }
	}
}
