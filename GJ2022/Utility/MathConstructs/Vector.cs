using System;

namespace GJ2022.Utility.MathConstructs
{
    public struct Vector
    {

        public Vector(int dimensions, params float[] values)
        {
            //Set the dimension of the vector
            Values = new float[dimensions];
            //Set the values
            for (int i = 0; i < values.Length; i++)
            {
                Values[i] = values[i];
            }
        }

        private float[] Values;

        /// <summary>
        /// Overload for the [] operator, returns the element of the vector.
        /// </summary>
        public float this[int x]
        {
            get { return Values[x]; }
            set { Values[x] = value; }
        }

        /// <summary>
        /// Gets the dimensions of the vector
        /// </summary>
        public int Dimensions
        {
            get { return Values.Length; }
        }

        public static bool operator ==(Vector a, Vector b)
        {
            if (a.Dimensions != b.Dimensions) return false;
            for (int i = 0; i < a.Dimensions; i++) if (a[i] != b[i]) return false;
            return true;
        }

        public static bool operator !=(Vector a, Vector b)
        {
            if (a.Dimensions != b.Dimensions) return true;
            for (int i = 0; i < a.Dimensions; i++) if (a[i] != b[i]) return true;
            return false;
        }

        /// <summary>
        /// Returns the vector multiplied by -1
        /// </summary>
        public static Vector operator -(Vector input) => input * -1;

        /// <summary>
        /// Returns the vector.
        /// </summary>
        public static Vector operator +(Vector input) => input;

        /// <summary>
        /// Adds 2 vectors together
        /// </summary>
        public static Vector operator +(Vector a, Vector b)
        {
            //Calculate the resulting dimensions of the new vector
            int resultingDimensions = Math.Max(a.Dimensions, b.Dimensions);
            //Create the new vector
            Vector vector = new Vector(resultingDimensions);
            //Add the values
            for (int i = 0; i < resultingDimensions; i++)
                vector[i] = (i < a.Dimensions ? a[i] : 0) + (i < b.Dimensions ? b[i] : 0);
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Adds a constant value b to all values of a
        /// </summary>
        public static Vector operator +(Vector a, float b)
        {
            //Create the new vector
            Vector vector = new Vector(a.Dimensions);
            //Add the values
            for (int i = 0; i < a.Dimensions; i++)
                vector[i] = a[i] + b;
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Takes the difference of 2 vectors
        /// </summary>
        public static Vector operator -(Vector a, Vector b)
        {
            //Calculate the resulting dimensions of the new vector
            int resultingDimensions = Math.Max(a.Dimensions, b.Dimensions);
            //Create the new vector
            Vector vector = new Vector(resultingDimensions);
            //Add the values
            for (int i = 0; i < resultingDimensions; i++)
                vector[i] = (i < a.Dimensions ? a[i] : 0) - (i < b.Dimensions ? b[i] : 0);
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Subtracts a constant value of b from all values of a.
        /// </summary>
        public static Vector operator -(Vector a, float b)
        {
            //Create the new vector
            Vector vector = new Vector(a.Dimensions);
            //Add the values
            for (int i = 0; i < a.Dimensions; i++)
                vector[i] = a[i] - b;
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Static wrapper for the dot product.
        /// Simply calls the dot product method on A with parameter B.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float DotProduct(Vector a, Vector b)
        {
            return a.DotProduct(b);
        }

        /// <summary>
        /// Calculates the dot product of this vector with another vector
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float DotProduct(Vector other)
        {
            float result = 0;
            //Add the values
            for (int i = 0; i < Math.Max(Dimensions, other.Dimensions); i++)
                result += this[i] * other[i];
            //Return the resulting vector
            return result;
        }

        /// <summary>
        /// Calculates the dot product of a and b
        /// </summary>
        public static Vector operator *(Vector a, Vector b)
        {
            //Calculate the resulting dimensions of the new vector
            int resultingDimensions = Math.Max(a.Dimensions, b.Dimensions);
            //Create the new vector
            Vector vector = new Vector(resultingDimensions);
            //Add the values
            for (int i = 0; i < resultingDimensions; i++)
                vector[i] = (i < a.Dimensions ? a[i] : 0) * (i < b.Dimensions ? b[i] : 0);
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Multiplies a by a scalar b
        /// </summary>
        public static Vector operator *(float b, Vector a) => a * b;
        public static Vector operator *(Vector a, float b)
        {
            //Create the new vector
            Vector vector = new Vector(a.Dimensions);
            //Add the values
            for (int i = 0; i < a.Dimensions; i++)
                vector[i] = a[i] * b;
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Divides the vector a by the values of the vector b
        /// </summary>
        public static Vector operator /(Vector a, Vector b)
        {
            //Calculate the resulting dimensions of the new vector
            int resultingDimensions = Math.Max(a.Dimensions, b.Dimensions);
            //Create the new vector
            Vector vector = new Vector(resultingDimensions);
            //Add the values
            for (int i = 0; i < resultingDimensions; i++)
                vector[i] = (i < a.Dimensions ? a[i] : 0) / (i < b.Dimensions ? b[i] : 0);
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Divides the values of a by the scalar b
        /// </summary>
        public static Vector operator /(Vector a, float b)
        {
            //Create the new vector
            Vector vector = new Vector(a.Dimensions);
            //Add the values
            for (int i = 0; i < a.Dimensions; i++)
                vector[i] = a[i] / b;
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Raises a to the power of b
        /// </summary>
        public static Vector operator ^(Vector a, float b)
        {
            //Create the new vector
            Vector vector = new Vector(a.Dimensions);
            //Add the values
            for (int i = 0; i < a.Dimensions; i++)
                vector[i] = (float)Math.Pow(a[i], b);
            //Return the resulting vector
            return vector;
        }

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        public float Length()
        {
            return (float)Math.Sqrt(DotProduct(this, this));
        }

        public override string ToString()
        {
            return $"{{{String.Join(", ", Values)}}}";
        }

        public override bool Equals(object obj)
        {
            return obj is Vector && (Vector)obj == this;
        }

        public override int GetHashCode()
        {
            int hashCode = -1466858141;
            foreach (int value in Values)
            {
                hashCode = unchecked(hashCode * 17 + value);
            }
            hashCode = hashCode * -1521134295 + Dimensions.GetHashCode();
            return hashCode;
        }

    }
}
