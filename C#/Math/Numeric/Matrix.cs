﻿using System;
using System.Text;

namespace Math.Numeric
{
    public abstract partial class Matrix<T> : ICloneable
        where T : struct
    {
        public int RowCount { get; }
        public int ColumnCount { get; }

        protected readonly T[,] Data;

        public T this[int row, int col]
        {
            get => At(row, col);
            set => At(row, col, value);
        }

        protected Matrix(int rows, int cols)
            : this(new T[rows, cols])
        { }

        protected Matrix(T[,] data) {
            Data = data;
            RowCount = Data.GetLength(0);
            ColumnCount = data.GetLength(1);
        }

        public T At(int row, int col)
        {
            return Data[row, col];
        }

        public void At(int row, int col, T value)
        {
            Data[row, col] = value;
        }

        public abstract object Clone();

        public Matrix<T> Add(T scalar)
        {
            var result = Build();

            DoAdd(scalar, result);

            return result;
        }

        public Matrix<T> Add(Matrix<T> other)
        {
            if(this.RowCount != other.RowCount || this.ColumnCount != other.ColumnCount)
                throw new Exception("Dimensions do not match");

            var result = Build();

            DoAdd(other, result);

            return result;
        }

        public Matrix<T> Substract(T scalar)
        {
            var result = Build();

            DoSubstract(scalar, result);

            return result;
        }

        public Matrix<T> SubstractFromScalar(T scalar)
        {
            var result = Build();

            DoSubstractFromScalar(scalar, result);

            return result;
        }

        public Matrix<T> Substract(Matrix<T> other)
        {
            if(this.RowCount != other.RowCount || this.ColumnCount != other.ColumnCount)
                throw new Exception("Dimensions do no match");

            var result = Build();

            DoSubstract(other, result);

            return result;
        }

        public Matrix<T> Multiply(T scalar)
        {
            var result = Build();

            DoMultiply(scalar, result);

            return result;
        }

        public Matrix<T> Multiply(Matrix<T> other)
        {
            if (this.ColumnCount != other.RowCount)
                throw new Exception("Dimensions do no match");

            var result = Build(RowCount, other.ColumnCount);

            DoMultiply(other, result);

            return result;
        }

        public Matrix<T> PointwiseMultiply(Matrix<T> other)
        {
            if(this.ColumnCount != other.ColumnCount || this.RowCount != other.RowCount)
                throw new Exception("Dimensions do no match");

            var result = Build(RowCount, ColumnCount);

            DoPointwiseMultiply(other, result);

            return result;
        }

        public Matrix<T> Transpose()
        {
            var result = Build(ColumnCount, RowCount);

            for (var i = 0; i < RowCount; i++) {
                for (var j = 0; j < ColumnCount; j++) {
                    result[j,i] = this[i,j];
                }
            }

            return result;
        }

        protected abstract Matrix<T> Build();

        protected abstract Matrix<T> Build(int rows, int cols);

        protected abstract void DoAdd(T scalar, Matrix<T> result);

        protected abstract void DoAdd(Matrix<T> other, Matrix<T> result);

        protected abstract void DoSubstract(T scalar, Matrix<T> other);

        protected abstract void DoSubstractFromScalar(T scalar, Matrix<T> other);

        protected abstract void DoSubstract(Matrix<T> other, Matrix<T> result);

        protected abstract void DoMultiply(T scalar, Matrix<T> result);

        protected abstract void DoMultiply(Matrix<T> other, Matrix<T> result);

        protected abstract void DoPointwiseMultiply(Matrix<T> other, Matrix<T> result);

        protected void Map(Matrix<T> result, Func<T, T> f)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    result[i, j] = f(this[i, j]);
                }
            }
        }

        protected void Map(Matrix<T> right, Matrix<T> result, Func<T, T, T> f)
        {
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColumnCount; j++)
                {
                    var r = f(this[i, j], right[i, j]);
                    result[i, j] = r;
                }
            }
        }
    }
}
