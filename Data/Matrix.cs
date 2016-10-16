using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathLib.Data {
	
	/// <summary>
	/// 行列 ※未検証クラス
	/// </summary>
	public class Matrix {
		private double[,] _matrix;
		private int _columns;
		private int _rows;
		
		/// <summary>
		/// 行数
		/// </summary>
		public int Rows{
			get{
				return _rows;
			}
		}

		/// <summary>
		/// 列数
		/// </summary>
		public int Columns {
			get{
				return _columns;
			}
		}

		public double this[int row, int col] {
			get{
				return _matrix[row, col];
			}
			set{
				_matrix[row, col] = value;
			}
		}

		/// <summary>
		/// 行列同士の積
		/// </summary>
		/// <param name="m1">m×nの行列</param>
		/// <param name="m2">n×pの行列</param>
		/// <returns></returns>
		public static Matrix operator * (Matrix m1, Matrix m2){
			if(m1.Columns != m2.Rows ){
				return null;
			}
			Matrix res = new Matrix(m1.Rows, m2.Columns);
			int n = m1.Columns;

			for(int i = 0; i < res.Rows; i++) {
				for(int j = 0; j < res.Columns; j++) {
					res[i, j]  = 0;
					for(int k = 0; k < n; k++){
						res[i, j] += m1[i, k] * m2[k, j];
					}
				}
			}

			return res;
		}

		public Matrix(int rows, int columns) {
			_matrix = new double[rows, columns];
			_rows = rows;
			_columns = columns;
		}

		/// <summary>
		/// 逆行列（掃き出し法）
		/// </summary>
		/// <returns></returns>
		public Matrix InverseRowReduction() {
			if(_rows == _columns) {
				///行＝＝列でない場合は計算不可
				return null;
			}

			Matrix invMat  = new Matrix(_rows, _columns);
			int n = _rows;

			double buf;

			///単位行列作成
			for(int i = 0; i < n; i++){
				for(int j = 0; j < n; j++){
					invMat[i, j] = (i == j) ? 1.0 : 0.0;
				}
			}

			double[,] src = new double[n, n];

			src.CopyTo(_matrix, 0);

			///掃き出し法
			for(int i = 0; i < n; i++) {
				buf = 1 / src[i, i];
				for(int j = 0; j < n; j++){
					src[i, j] *= buf;
					invMat[i, j] *= buf;
				}

				for(int j=0; j<n; j++){
					if(i != j){
						buf = src[j, i];
						for(int k = 0; k < n; k++){
							src[j, k] -= src[i, k] * buf;
							invMat[j, k] -= invMat[i, k] * buf;
						}
					}
				}
			}

			return invMat;
		}

		public void CopyFrom(Matrix mat){
			_rows = mat._rows;
			_columns = mat._columns;
			_matrix = new double[_rows, _columns];
			_matrix.CopyTo(mat._matrix, 0);
		}
	}
}
