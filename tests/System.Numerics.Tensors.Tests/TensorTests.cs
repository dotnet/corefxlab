// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Numerics;
using Xunit;

namespace tests
{
    public class TensorTests
    {
        [Fact]
        public void ConstructTensorFromArrayRank1()
        {
            // use array to avoid calling params int[] overload
            Array array = new[] { 0, 1, 2 };
            var tensor = new Tensor<int>(array);

            Assert.Equal(0, tensor[0]);
            Assert.Equal(1, tensor[1]);
            Assert.Equal(2, tensor[2]);
        }

        [Fact]
        public void ConstructTensorFromArrayRank2()
        {
            var tensor = new Tensor<int>(new[,]
            {
                {0, 1, 2},
                {3, 4, 5}
            });

            Assert.Equal(0, tensor[0, 0]);
            Assert.Equal(1, tensor[0, 1]);
            Assert.Equal(2, tensor[0, 2]);
            Assert.Equal(3, tensor[1, 0]);
            Assert.Equal(4, tensor[1, 1]);
            Assert.Equal(5, tensor[1, 2]);
        }

        [Fact]
        public void TestDefaultConstructor()
        {
            Tensor<int> defaultTensor = default(Tensor<int>);

            Assert.Equal(0, defaultTensor.Buffer.Length);
            Assert.Equal(0, defaultTensor.Dimensions.Count);
            Assert.True(defaultTensor.IsFixedSize);
            Assert.False(defaultTensor.IsReadOnly);
            Assert.Equal(0, defaultTensor.Length);
            Assert.Equal(0, defaultTensor.Rank);

            // don't throw
            var clone = defaultTensor.Clone();
            clone = defaultTensor.CloneEmpty();
            var doubleClone = defaultTensor.CloneEmpty<double>();
            var arr = new int[0];
            defaultTensor.CopyTo(arr, 0);
            defaultTensor.Fill(0);
            defaultTensor.Reshape();
            defaultTensor.ReshapeCopy();
            defaultTensor.ToString();

            Assert.Equal(default(Tensor<int>), defaultTensor);
            Assert.Equal(default(Tensor<int>).GetHashCode(), defaultTensor.GetHashCode());
            Assert.False(defaultTensor.GetEnumerator().MoveNext());

            // rank is too small
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetDiagonal());
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetTriangle());
            Assert.Throws<InvalidOperationException>(() => defaultTensor.GetUpperTriangle());

            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0, 0]);
            Assert.Throws<ArgumentOutOfRangeException>(() => defaultTensor[0, 0, 0]);
            
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor + 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor + defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => +defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor - 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor - defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => -defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor++);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor--);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor * 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor * defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor / 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor / defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor % 1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor % defaultTensor);
            Assert.Throws<ArgumentException>("left", () => defaultTensor & defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor & -1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor | defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor | -1);
            Assert.Throws<ArgumentException>("left", () => defaultTensor ^ defaultTensor);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor ^ -1);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor >> 1);
            Assert.Throws<ArgumentException>("tensor", () => defaultTensor << 1);
        }

        [Fact]
        public void ConstructTensorFromArrayRank3()
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new Tensor<int>(arr);

            Assert.Equal(0, tensor[0, 0, 0]);
            Assert.Equal(1, tensor[0, 0, 1]);
            Assert.Equal(2, tensor[0, 0, 2]);
            Assert.Equal(3, tensor[0, 1, 0]);
            Assert.Equal(4, tensor[0, 1, 1]);
            Assert.Equal(5, tensor[0, 1, 2]);

            Assert.Equal(6, tensor[1, 0, 0]);
            Assert.Equal(7, tensor[1, 0, 1]);
            Assert.Equal(8, tensor[1, 0, 2]);
            Assert.Equal(9, tensor[1, 1, 0]);
            Assert.Equal(10, tensor[1, 1, 1]);
            Assert.Equal(11, tensor[1, 1, 2]);

            Assert.Equal(12, tensor[2, 0, 0]);
            Assert.Equal(13, tensor[2, 0, 1]);
            Assert.Equal(14, tensor[2, 0, 2]);
            Assert.Equal(15, tensor[2, 1, 0]);
            Assert.Equal(16, tensor[2, 1, 1]);
            Assert.Equal(17, tensor[2, 1, 2]);

            Assert.Equal(18, tensor[3, 0, 0]);
            Assert.Equal(19, tensor[3, 0, 1]);
            Assert.Equal(20, tensor[3, 0, 2]);
            Assert.Equal(21, tensor[3, 1, 0]);
            Assert.Equal(22, tensor[3, 1, 1]);
            Assert.Equal(23, tensor[3, 1, 2]);

        }

        [Fact]
        public void ConstructTensorFromArrayRank3WithLowerBounds()
        {
            var arrayWithLowerBounds = Array.CreateInstance(typeof(int), new[] { 2, 3, 4 }, new[] { 0, 5, 200 });


            
        }

        [Fact]
        public void StructurallyEqualTensor()
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new Tensor<int>(arr);
            var tensor2 = new Tensor<int>(arr);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, tensor2));
            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor2, tensor));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, tensor2));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor2, tensor));
            Assert.Equal(StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor), StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor2));
        }


        [Fact]
        public void StructurallyEqualArray()
        {
            var arr = new[, ,]
            {
                {
                    {0, 1, 2},
                    {3, 4, 5}
                },
                {
                    {6, 7 ,8 },
                    {9, 10 ,11 },
                },
                {
                    {12, 13 ,14 },
                    {15, 16 ,17 },
                },
                {
                    {18, 19 ,20 },
                    {21, 22 ,23 },
                }
            };
            var tensor = new Tensor<int>(arr);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, arr));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, arr));

        }

        [Fact]
        public void GetDiagonalSquare()
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new Tensor<int>(arr);
            var diag = tensor.GetDiagonal();
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1, 3, 5 }));
            diag = tensor.GetDiagonal(1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 2, 9 }));
            diag = tensor.GetDiagonal(2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 4 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(3));

            diag = tensor.GetDiagonal(-1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 8, 7 }));
            diag = tensor.GetDiagonal(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-3));
        }

        [Fact]
        public void GetDiagonalRectangle()
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new Tensor<int>(arr);
            var diag = tensor.GetDiagonal();
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1, 3, 5 }));
            diag = tensor.GetDiagonal(1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 2, 9, 2 }));
            diag = tensor.GetDiagonal(2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 4, 2, 9 }));
            diag = tensor.GetDiagonal(3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 3, 6 }));
            diag = tensor.GetDiagonal(4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 7 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(5));

            diag = tensor.GetDiagonal(-1);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 8, 7 }));
            diag = tensor.GetDiagonal(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, new[] { 1 }));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-3));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-4));
            Assert.Throws<ArgumentException>("offset", () => tensor.GetDiagonal(-5));
        }


        [Fact]
        public void GetDiagonalCube()
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new Tensor<int>(arr);
            var diag = tensor.GetDiagonal();
            var expected = new[,]
            {
                { 1, 2, 4 },
                { 1, 6, 2 },
                { 4, 9, 4 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, expected));
        }

        [Fact]
        public void GetTriangleSquare()
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetTriangle(0);

            var expected = new Tensor<int>(new[,]
            {
               { 1, 0, 0 },
               { 8, 3, 0 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 0 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(200);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-1);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 8, 0, 0 },
               { 1, 7, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-2);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 1, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));


            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            tri = tensor.GetTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            // same as -3, should it be an exception?
            tri = tensor.GetTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-300);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Fact]
        public void GetTriangleRectangle()
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetTriangle(0);
            var expected = new Tensor<int>(new[,]
            {
               { 1, 0, 0, 0, 0 },
               { 8, 3, 0, 0, 0 },
               { 1, 7, 5, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 0, 0, 0 },
               { 8, 3, 9, 0, 0 },
               { 1, 7, 5, 2, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 0, 0 },
               { 8, 3, 9, 2, 0 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(3);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 3, 0 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(4);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            // same as 4, should it be an exception?
            tri = tensor.GetTriangle(5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1000);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-1);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 8, 0, 0, 0, 0 },
               { 1, 7, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 1, 0, 0, 0, 0 }
            });
            tri = tensor.GetTriangle(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            tri = tensor.GetTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-100);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Fact]
        public void GetTriangleCube()
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetTriangle(0);
            var expected = new Tensor<int>(new[,,]
            {
                {
                   { 1, 2, 4 },
                   { 0, 0, 0 },
                   { 0, 0, 0 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 0, 0, 0 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Fact]
        public void GetUpperTriangleSquare()
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetUpperTriangle(0);

           var expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 0, 3, 9 },
               { 0, 0, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(1);
            expected = new Tensor<int>(new[,]
            {
               { 0, 2, 4 },
               { 0, 0, 9 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 4 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(3);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(42);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-1);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 0, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-2);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-300);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }
        
        [Fact]
        public void GetUpperTriangleRectangle()
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetUpperTriangle(0);
            var expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 0, 3, 9, 2, 6 },
               { 0, 0, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(1);
            expected = new Tensor<int>(new[,]
            {
               { 0, 2, 4, 3, 7 },
               { 0, 0, 9, 2, 6 },
               { 0, 0, 0, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 4, 3, 7 },
               { 0, 0, 0, 2, 6 },
               { 0, 0, 0, 0, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(3);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 3, 7 },
               { 0, 0, 0, 0, 6 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(4);
            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 0, 7 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new Tensor<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            tri = tensor.GetUpperTriangle(5);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(6);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(1000);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(-1);
            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 0, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = new Tensor<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            tri = tensor.GetUpperTriangle(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            
            tri = tensor.GetUpperTriangle(-3);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-4);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-100);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }
        
        [Fact]
        public void GetUpperTriangleCube()
        {
            var arr = new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 4, 5, 7 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 5, 6, 1 },
                   { 2, 2, 3 },
                   { 4, 9, 4 },
                },

            };

            var tensor = new Tensor<int>(arr);
            var tri = tensor.GetUpperTriangle(0);
            var expected = new Tensor<int>(new[, ,]
            {
                {
                   { 1, 2, 4 },
                   { 8, 3, 9 },
                   { 1, 7, 5 },
                },
                {
                   { 0, 0, 0 },
                   { 1, 6, 2 },
                   { 3, 0, 8 },
                },
                {
                   { 0, 0, 0 },
                   { 0, 0, 0 },
                   { 4, 9, 4 },
                },

            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
        }

        [Fact]
        public void Reshape()
        {
            var arr = new[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            var tensor = new Tensor<int>(arr);
            var actual = tensor.Reshape(3, 2);

            var expected = new[,]
            {
                { 1, 2 },
                { 3, 4 },
                { 5, 6 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Identity()
        {
            var actual = Tensor<double>.CreateIdentity(3);

            var expected = new[,]
            {
                {1.0, 0, 0 },
                {0, 1.0, 0 },
                {0, 0, 1.0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void CreateWithDiagonal()
        {
            var actual = Tensor<int>.CreateFromDiagonal(new Tensor<int>(fromArray: new[] { 1, 2, 3, 4, 5 }));

            var expected = new[,]
            {
                {1, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0 },
                {0, 0, 3, 0, 0 },
                {0, 0, 0, 4, 0 },
                {0, 0, 0, 0, 5 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void CreateWithDiagonalAndOffset()
        {
            var actual = Tensor<int>.CreateFromDiagonal(new Tensor<int>(fromArray: new[] { 1, 2, 3, 4 }), 1);

            var expected = new[,]
            {
                {0, 1, 0, 0, 0 },
                {0, 0, 2, 0, 0 },
                {0, 0, 0, 3, 0 },
                {0, 0, 0, 0, 4 },
                {0, 0, 0, 0, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new Tensor<int>(fromArray: new[] { 1, 2, 3, 4 }), -1);

            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0 },
                {0, 0, 3, 0, 0 },
                {0, 0, 0, 4, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new Tensor<int>(fromArray: new[] { 1 }), -4);
            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            actual = Tensor<int>.CreateFromDiagonal(new Tensor<int>(fromArray: new[] { 1 }), 4);
            expected = new[,]
            {
                {0, 0, 0, 0, 1 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Add()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = new Tensor<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    { 6, 8, 10 },
                    { 12, 14, 16 },
                });

            var actual = left + right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

        }

        [Fact]
        public void AddScalar()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                });

            var actual = tensor + 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

        }

        [Fact]
        public void UnaryPlus()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensor;

            var actual = +tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }


        [Fact]
        public void Subtract()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = new Tensor<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    { -6, -6, -6 },
                    { -6, -6, -6},
                });

            var actual = left - right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

        }

        [Fact]
        public void SubtractScalar()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    { -1, 0, 1 },
                    { 2, 3, 4 },
                });

            var actual = tensor - 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void UnaryMinus()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, -1, -2},
                    {-3, -4, -5}
                });

            var actual = -tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void PrefixIncrement()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expectedResult = new Tensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                });

            var expectedTensor = expectedResult;

            var actual = ++tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
        }


        [Fact]
        public void PostfixIncrement()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // returns original value
            var expectedResult = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // increments operand
            var expectedTensor = new Tensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }); ;

            var actual = tensor++;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
        }


        [Fact]
        public void PrefixDecrement()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expectedResult = new Tensor<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                });

            var expectedTensor = expectedResult;

            var actual = --tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
        }

        [Fact]
        public void PostfixDecrement()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // returns original value
            var expectedResult = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // decrements operand
            var expectedTensor = new Tensor<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                }); ;

            var actual = tensor--;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
        }

        [Fact]
        public void Multiply()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                });

            var actual = tensor * tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void MultiplyScalar()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = tensor * 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Divide()
        {
            var dividend = new Tensor<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                });

            var divisor = new Tensor<int>(
                new[,]
                {
                    {1, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend / divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void DivideScalar()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = tensor / 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Modulo()
        {
            var dividend = new Tensor<int>(
                new[,]
                {
                    {0, 3, 8},
                    {11, 14, 17}
                });

            var divisor = new Tensor<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend % divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void ModuloScalar()
        {
            var tensor = new Tensor<int>(
                new[,]
                {
                    {0, 3, 4},
                    {7, 8, 9}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 0},
                    {1, 0, 1}
                });

            var actual = tensor % 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void And()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 15, 31}
                });

            var right = new Tensor<int>(
                new[,]
                {
                    {1, 1, 3},
                    {2, 4, 8}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {2, 4, 8}
                });

            var actual = left & right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void AndScalar()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {5, 15, 31}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 0, 0},
                    {4, 4, 20}
                });

            var actual = left & 20;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Or()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                });

            var right = new Tensor<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {1, 3, 7},
                    {7, 14, 31}
                });

            var actual = left | right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void OrScalar()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {1, 1, 3},
                    {3, 5, 5}
                });

            var actual = left | 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void Xor()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                });

            var right = new Tensor<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {1, 3, 7},
                    {5, 10, 23}
                });

            var actual = left ^ right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void XorScalar()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {1, 0, 3},
                    {2, 5, 4}
                });

            var actual = left ^ 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }
        
        [Fact]
        public void LeftShift()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = left << 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void RightShift()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = new Tensor<int>(
                new[,]
                {
                    {0, 0, 1},
                    {1, 2, 2}
                });

            var actual = left >> 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Fact]
        public void ElementWiseEquals()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = new Tensor<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                });

            var expected = new Tensor<bool>(
                new[,]
                {
                    {true, true, false },
                    {false, false, true}
                });

            var actual = Tensor.Equals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }


        [Fact]
        public void ElementWiseNotEquals()
        {
            var left = new Tensor<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = new Tensor<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                });

            var expected = new Tensor<bool>(
                new[,]
                {
                    {false, false, true},
                    {true, true, false}
                });

            var actual = Tensor.NotEquals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

    }
}
