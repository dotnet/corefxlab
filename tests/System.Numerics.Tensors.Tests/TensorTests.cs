// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Xunit;

namespace tests
{
    public class TensorTests
    {
        public enum TensorType
        {
            Dense,
            Sparse,
            CompressedSparse
        };

        public class TensorConstructor
        {
            public TensorType TensorType { get; set; }

            public bool IsReversedStride { get; set; }

            public Tensor<T> CreateFromArray<T>(Array array)
            {
                switch (TensorType)
                {
                    case TensorType.Dense:
                        return new DenseTensor<T>(array, IsReversedStride);
                    case TensorType.Sparse:
                        return new SparseTensor<T>(array, IsReversedStride);
                    case TensorType.CompressedSparse:
                        return new CompressedSparseTensor<T>(array, IsReversedStride);
                }

                throw new ArgumentException(nameof(TensorType));
            }
            public Tensor<T> CreateFromDimensions<T>(int[] dimensions)
            {
                switch (TensorType)
                {
                    case TensorType.Dense:
                        return new DenseTensor<T>(dimensions, IsReversedStride);
                    case TensorType.Sparse:
                        return new SparseTensor<T>(dimensions, IsReversedStride);
                    case TensorType.CompressedSparse:
                        return new CompressedSparseTensor<T>(dimensions, IsReversedStride);
                }

                throw new ArgumentException(nameof(TensorType));
            }

            public override string ToString()
            {
                return $"{TensorType}, {nameof(IsReversedStride)} = {IsReversedStride}";
            }
        }

        private static TensorType[] s_tensorTypes = new[]
        {
            TensorType.Dense,
            TensorType.Sparse,
            TensorType.CompressedSparse
        };

        private static bool[] s_reverseStrideValues = new[]
        {
            false,
            true
        };

        public static IEnumerable<object[]> GetSingleTensorConstructors()
        {
            foreach (TensorType tensorType in s_tensorTypes)
            {
                foreach (bool isReversedStride in s_reverseStrideValues)
                {
                    yield return new[]
                    {
                        new TensorConstructor()
                        {
                            TensorType = tensorType,
                            IsReversedStride = isReversedStride
                        }
                    };
                }
            }
        }


        public static IEnumerable<object[]> GetDualTensorConstructors()
        {
            foreach (TensorType leftTensorType in s_tensorTypes)
            {
                foreach (TensorType rightTensorType in s_tensorTypes)
                {
                    foreach (bool isLeftReversedStride in s_reverseStrideValues)
                    {
                        foreach (bool isRightReversedStride in s_reverseStrideValues)
                        {
                            yield return new[]
                            {
                                new TensorConstructor()
                                {
                                    TensorType = leftTensorType,
                                    IsReversedStride = isLeftReversedStride
                                },
                                new TensorConstructor()
                                {
                                    TensorType = rightTensorType,
                                    IsReversedStride = isRightReversedStride
                                }
                            };
                        }
                    }
                }
            }
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructTensorFromArrayRank1(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(new[] { 0, 1, 2 });

            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);
            Assert.Equal(0, tensor[0]);
            Assert.Equal(1, tensor[1]);
            Assert.Equal(2, tensor[2]);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructTensorFromArrayRank2(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(new[,]
            {
                {0, 1, 2},
                {3, 4, 5}
            });

            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);
            Assert.Equal(0, tensor[0, 0]);
            Assert.Equal(1, tensor[0, 1]);
            Assert.Equal(2, tensor[0, 2]);
            Assert.Equal(3, tensor[1, 0]);
            Assert.Equal(4, tensor[1, 1]);
            Assert.Equal(5, tensor[1, 2]);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructTensorFromArrayRank3(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(new[, ,]
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
            });

            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);

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
        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructSparseTensor(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(new[,]
            {
                {0, 0, 0, 0},
                {5, 8, 0, 0},
                {0, 0, 3, 0},
                {0, 6, 0, 0}
            });

            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);


            Assert.Equal(0, tensor[0, 0]);
            Assert.Equal(0, tensor[0, 1]);
            Assert.Equal(0, tensor[0, 2]);
            Assert.Equal(0, tensor[0, 3]);


            Assert.Equal(5, tensor[1, 0]);
            Assert.Equal(8, tensor[1, 1]);
            Assert.Equal(0, tensor[1, 2]);
            Assert.Equal(0, tensor[1, 3]);


            Assert.Equal(0, tensor[2, 0]);
            Assert.Equal(0, tensor[2, 1]);
            Assert.Equal(3, tensor[2, 2]);
            Assert.Equal(0, tensor[2, 3]);


            Assert.Equal(0, tensor[3, 0]);
            Assert.Equal(6, tensor[3, 1]);
            Assert.Equal(0, tensor[3, 2]);
            Assert.Equal(0, tensor[3, 3]);

            if (tensorConstructor.TensorType == TensorType.CompressedSparse)
            {
                var compressedSparseTensor = (CompressedSparseTensor<int>)tensor;

                Assert.Equal(4, compressedSparseTensor.ValueCount);

                int[] expectedValues, expectedCompressedCounts, expectedIndices;

                if (compressedSparseTensor.IsReversedStride)
                {
                    // csc
                    expectedValues = new[] { 5, 8, 6, 3 };
                    expectedCompressedCounts = new[] { 0, 1, 3, 4, 4 };
                    expectedIndices = new[] { 1, 1, 3, 2 };
                }
                else
                {
                    // csr
                    expectedValues = new[] { 5, 8, 3, 6 };
                    expectedCompressedCounts = new[] { 0, 0, 2, 3, 4 };
                    expectedIndices = new[] { 0, 1, 2, 1 };
                }
                Assert.Equal(expectedValues, compressedSparseTensor.Values.Take(compressedSparseTensor.ValueCount));
                Assert.Equal(expectedCompressedCounts, compressedSparseTensor.CompressedCounts);
                Assert.Equal(expectedIndices, compressedSparseTensor.Indices.Take(compressedSparseTensor.ValueCount));
            }
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructFromDimensions(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromDimensions<int>(new[] { 2, 3, 4 });
            Assert.Equal(3, tensor.Rank);
            Assert.Equal(3, tensor.Dimensions.Count);
            Assert.Equal(2, tensor.Dimensions[0]);
            Assert.Equal(3, tensor.Dimensions[1]);
            Assert.Equal(4, tensor.Dimensions[2]);
            Assert.Equal(24, tensor.Length);
            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);

            Assert.Throws<ArgumentNullException>("dimensions", () => tensorConstructor.CreateFromDimensions<int>(dimensions: null));
            Assert.Throws<ArgumentException>("dimensions", () => tensorConstructor.CreateFromDimensions<int>(dimensions: new int[0]));

            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => tensorConstructor.CreateFromDimensions<int>(dimensions: new[] { 1, 0 }));
            Assert.Throws<ArgumentOutOfRangeException>("dimensions", () => tensorConstructor.CreateFromDimensions<int>(dimensions: new[] { 1, -1 }));

            // ensure dimensions are immutable
            var dimensions = new[] { 1, 2, 3 };
            tensor = tensorConstructor.CreateFromDimensions<int>(dimensions:dimensions);
            dimensions[0] = dimensions[1] = dimensions[2] = 0;
            Assert.Equal(1, tensor.Dimensions[0]);
            Assert.Equal(2, tensor.Dimensions[1]);
            Assert.Equal(3, tensor.Dimensions[2]);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ConstructTensorFromArrayRank3WithLowerBounds(TensorConstructor tensorConstructor)
        {
            var dimensions = new[] { 2, 3, 4 };
            var lowerBounds = new[] { 0, 5, 200 };
            var arrayWithLowerBounds = Array.CreateInstance(typeof(int), dimensions, lowerBounds);

            int value = 0;
            for(int x = lowerBounds[0]; x < lowerBounds[0] + dimensions[0]; x++)
            {
                for (int y = lowerBounds[1]; y < lowerBounds[1] + dimensions[1]; y++)
                {
                    for (int z = lowerBounds[2]; z < lowerBounds[2] + dimensions[2]; z++)
                    {
                        arrayWithLowerBounds.SetValue(value++, x, y, z);
                    }
                }
            }

            var tensor = tensorConstructor.CreateFromArray<int>(arrayWithLowerBounds);

            var expected = tensorConstructor.CreateFromArray<int>(new[,,]
                    {
                        {
                            { 0, 1, 2, 3 },
                            { 4, 5, 6, 7 },
                            { 8, 9, 10, 11 }
                        },
                        {
                            { 12, 13, 14, 15 },
                            { 16, 17, 18, 19 },
                            { 20, 21, 22, 23 }
                        }
                    }
                );
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(expected, tensor));
            Assert.Equal(tensorConstructor.IsReversedStride, tensor.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void StructurallyEqualTensor(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
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
            var tensor = leftConstructor.CreateFromArray<int>(arr);
            var tensor2 = rightConstructor.CreateFromArray<int>(arr);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, tensor2));
            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor2, tensor));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, tensor2));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor2, tensor));
            // Issue: should Tensors with different layout be structurally equal?
            if (leftConstructor.IsReversedStride == leftConstructor.IsReversedStride)
            {
                Assert.Equal(StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor), StructuralComparisons.StructuralEqualityComparer.GetHashCode(tensor2));
            }
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void StructurallyEqualArray(TensorConstructor tensorConstructor)
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
            var tensor = tensorConstructor.CreateFromArray<int>(arr);

            Assert.Equal(0, StructuralComparisons.StructuralComparer.Compare(tensor, arr));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, arr));

        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetDiagonalSquare(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
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

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetDiagonalRectangle(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
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


        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetDiagonalCube(TensorConstructor tensorConstructor)
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

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var diag = tensor.GetDiagonal();
            var expected = new[,]
            {
                { 1, 2, 4 },
                { 1, 6, 2 },
                { 4, 9, 4 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(diag, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, diag.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetTriangleSquare(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetTriangle(0);
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);

            var expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 0, 0 },
               { 8, 3, 0 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(1);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 0 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
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
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0 },
               { 8, 0, 0 },
               { 1, 7, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(-2);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0 },
               { 0, 0, 0 },
               { 1, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));


            expected =  tensorConstructor.CreateFromArray<int>(new[,]
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

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetTriangleRectangle(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetTriangle(0);
            var expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 0, 0, 0, 0 },
               { 8, 3, 0, 0, 0 },
               { 1, 7, 5, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);

            tri = tensor.GetTriangle(1);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 0, 0, 0 },
               { 8, 3, 9, 0, 0 },
               { 1, 7, 5, 2, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(2);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4, 0, 0 },
               { 8, 3, 9, 2, 0 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetTriangle(3);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4, 3, 0 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetTriangle(4);
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
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
            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 8, 0, 0, 0, 0 },
               { 1, 7, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected =  tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 },
               { 1, 0, 0, 0, 0 }
            });
            tri = tensor.GetTriangle(-2);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected =  tensorConstructor.CreateFromArray<int>(new[,]
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

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetTriangleCube(TensorConstructor tensorConstructor)
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

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetTriangle(0);
            var expected = tensorConstructor.CreateFromArray<int>(new[,,]
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
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetUpperTriangleSquare(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 1, 7, 5 },
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetUpperTriangle(0);

           var expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4 },
               { 0, 3, 9 },
               { 0, 0, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);

            tri = tensor.GetUpperTriangle(1);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 2, 4 },
               { 0, 0, 9 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 4 },
               { 0, 0, 0 },
               { 0, 0, 0 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(3);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
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
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4 },
               { 8, 3, 9 },
               { 0, 7, 5 },
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(-2);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
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

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetUpperTriangleRectangle(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 1, 7, 5, 2, 9 }
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetUpperTriangle(0);
            var expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 0, 3, 9, 2, 6 },
               { 0, 0, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);
            tri = tensor.GetUpperTriangle(1);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 2, 4, 3, 7 },
               { 0, 0, 9, 2, 6 },
               { 0, 0, 0, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(2);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 4, 3, 7 },
               { 0, 0, 0, 2, 6 },
               { 0, 0, 0, 0, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));
            tri = tensor.GetUpperTriangle(3);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0, 3, 7 },
               { 0, 0, 0, 0, 6 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            tri = tensor.GetUpperTriangle(4);
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 0, 0, 0, 0, 7 },
               { 0, 0, 0, 0, 0 },
               { 0, 0, 0, 0, 0 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = tensorConstructor.CreateFromArray<int>(new[,]
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
            expected = tensorConstructor.CreateFromArray<int>(new[,]
            {
               { 1, 2, 4, 3, 7 },
               { 8, 3, 9, 2, 6 },
               { 0, 7, 5, 2, 9 }
            });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tri, expected));

            expected = tensorConstructor.CreateFromArray<int>(new[,]
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

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetUpperTriangleCube(TensorConstructor tensorConstructor)
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

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var tri = tensor.GetUpperTriangle(0);
            var expected = tensorConstructor.CreateFromArray<int>(new[, ,]
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
            Assert.Equal(tensorConstructor.IsReversedStride, tri.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void Reshape(TensorConstructor tensorConstructor)
        {
            var arr = new[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 }
            };

            var tensor = tensorConstructor.CreateFromArray<int>(arr);
            var actual = tensor.Reshape(3, 2);

            var expected = tensorConstructor.IsReversedStride ?
                new[,]
                {
                    { 1, 5 },
                    { 4, 3 },
                    { 2, 6 }
                } :
                new[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 }
                };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
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

        [Theory]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void CreateWithDiagonal(TensorConstructor tensorConstructor)
        {
            var diagonal = tensorConstructor.CreateFromArray<int>(new[] { 1, 2, 3, 4, 5 });
            var actual = Tensor<int>.CreateFromDiagonal(diagonal);

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

        [Theory]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void CreateWithDiagonalAndOffset(TensorConstructor tensorConstructor)
        {
            var diagonal = tensorConstructor.CreateFromArray<int>(new[] { 1, 2, 3, 4 });
            var actual = Tensor<int>.CreateFromDiagonal(diagonal, 1);

            var expected = new[,]
            {
                {0, 1, 0, 0, 0 },
                {0, 0, 2, 0, 0 },
                {0, 0, 0, 3, 0 },
                {0, 0, 0, 0, 4 },
                {0, 0, 0, 0, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            diagonal = tensorConstructor.CreateFromArray<int>(new[] { 1, 2, 3, 4 });
            actual = Tensor<int>.CreateFromDiagonal(diagonal, -1);

            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 },
                {0, 2, 0, 0, 0 },
                {0, 0, 3, 0, 0 },
                {0, 0, 0, 4, 0 }
            };

            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            diagonal = tensorConstructor.CreateFromArray<int>(new[] { 1 });
            actual = Tensor<int>.CreateFromDiagonal(diagonal, -4);
            expected = new[,]
            {
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {0, 0, 0, 0, 0 },
                {1, 0, 0, 0, 0 }
            };
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            diagonal = tensorConstructor.CreateFromArray<int>(new[] { 1 });
            actual = Tensor<int>.CreateFromDiagonal(diagonal, 4);
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

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Add(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 6, 8, 10 },
                    { 12, 14, 16 },
                });

            var actual = left + right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);

        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void AddScalar(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 1, 2, 3 },
                    { 4, 5, 6 },
                });

            var actual = tensor + 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);

        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void UnaryPlus(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensor;

            var actual = +tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(false, ReferenceEquals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }


        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Subtract(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 6, 7 ,8 },
                    { 9, 10 ,11 },
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    { -6, -6, -6 },
                    { -6, -6, -6},
                });

            var actual = left - right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void SubtractScalar(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    { -1, 0, 1 },
                    { 2, 3, 4 },
                });

            var actual = tensor - 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void UnaryMinus(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, -1, -2},
                    {-3, -4, -5}
                });

            var actual = -tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(false, ReferenceEquals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void PrefixIncrement(TensorConstructor tensorConstructor)
        {
            Tensor<int> tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expectedResult = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                });

            var expectedTensor = expectedResult;

            var actual = ++tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(true, ReferenceEquals(tensor, actual));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }


        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void PostfixIncrement(TensorConstructor tensorConstructor)
        {
            Tensor<int> tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // returns original value
            var expectedResult = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // increments operand
            var expectedTensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                }); ;

            var actual = tensor++;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(false, ReferenceEquals(tensor, actual));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }


        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void PrefixDecrement(TensorConstructor tensorConstructor)
        {
            Tensor<int> tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expectedResult = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                });

            var expectedTensor = expectedResult;

            var actual = --tensor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(true, ReferenceEquals(tensor, actual));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void PostfixDecrement(TensorConstructor tensorConstructor)
        {
            Tensor<int> tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // returns original value
            var expectedResult = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            // decrements operand
            var expectedTensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {-1, 0, 1},
                    {2, 3, 4}
                }); ;

            var actual = tensor--;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expectedResult));
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(tensor, expectedTensor));
            Assert.Equal(false, ReferenceEquals(tensor, actual));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Multiply(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                });

            var actual = left * right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void MultiplyScalar(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = tensor * 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Divide(TensorConstructor dividendConstructor, TensorConstructor divisorConstructor)
        {
            var dividend = dividendConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 4},
                    {9, 16, 25}
                });

            var divisor = divisorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 1, 2},
                    {3, 4, 5}
                });

            var expected = divisorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend / divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(dividendConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void DivideScalar(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = tensor / 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Modulo(TensorConstructor dividendConstructor, TensorConstructor divisorConstructor)
        {
            var dividend = dividendConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 3, 8},
                    {11, 14, 17}
                });

            var divisor = divisorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6}
                });

            var expected = dividendConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var actual = dividend % divisor;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(dividendConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void ModuloScalar(TensorConstructor tensorConstructor)
        {
            var tensor = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 3, 4},
                    {7, 8, 9}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 0},
                    {1, 0, 1}
                });

            var actual = tensor % 2;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void And(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 15, 31}
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 1, 3},
                    {2, 4, 8}
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 3},
                    {2, 4, 8}
                });

            var actual = left & right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void AndScalar(TensorConstructor tensorConstructor)
        {
            var left = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 3},
                    {5, 15, 31}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 0, 0},
                    {4, 4, 20}
                });

            var actual = left & 20;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Or(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 3, 7},
                    {7, 14, 31}
                });

            var actual = left | right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void OrScalar(TensorConstructor tensorConstructor)
        {
            var left = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 1, 3},
                    {3, 5, 5}
                });

            var actual = left | 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Xor(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 3},
                    {7, 14, 31}
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 4},
                    {2, 4, 8}
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 3, 7},
                    {5, 10, 23}
                });

            var actual = left ^ right;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void XorScalar(TensorConstructor tensorConstructor)
        {
            var left = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 0, 3},
                    {2, 5, 4}
                });

            var actual = left ^ 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void LeftShift(TensorConstructor tensorConstructor)
        {
            var left = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 2, 4},
                    {6, 8, 10}
                });

            var actual = left << 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void RightShift(TensorConstructor tensorConstructor)
        {
            var left = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var expected = tensorConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 0, 1},
                    {1, 2, 2}
                });

            var actual = left >> 1;
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(tensorConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void ElementWiseEquals(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                });

            var expected = new DenseTensor<bool>(
                new[,]
                {
                    {true, true, false },
                    {false, false, true}
                });

            var actual = Tensor.Equals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }
        
        [Theory()]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void ElementWiseNotEquals(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });
            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, -2},
                    {2, 3, 5}
                });

            var expected = new DenseTensor<bool>(
                new[,]
                {
                    {false, false, true},
                    {true, true, false}
                });

            var actual = Tensor.NotEquals(left, right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
            Assert.Equal(leftConstructor.IsReversedStride, actual.IsReversedStride);
        }

        [Theory]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void MatrixMultiply(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2},
                    {3, 4, 5}
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0, 1, 2, 3, 4},
                    {5, 6, 7, 8, 9},
                    {10, 11, 12, 13, 14}
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0*0 + 1*5 + 2*10, 0*1 + 1*6 + 2*11, 0*2 + 1*7 + 2*12, 0*3 + 1*8 + 2*13, 0*4 + 1*9 + 2*14},
                    {3*0 + 4*5 + 5*10, 3*1 + 4*6 + 5*11, 3*2 + 4*7 + 5*12, 3*3 + 4*8 + 5*13, 3*4 + 4*9 + 5*14}
                });

            var actual = left.MatrixMultiply(right);
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }



        [Theory]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void Contract(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[, ,]
                {
                    {
                        {0, 1},
                        {2, 3}
                    },
                    {
                        {4, 5},
                        {6, 7}
                    },
                    {
                        {8, 9},
                        {10, 11}
                    }
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[, ,]
                {
                    {
                        {0, 1},
                        {2, 3},
                        {4, 5}
                    },
                    {
                        {6, 7},
                        {8, 9},
                        {10, 11}
                    },
                    {
                        {12, 13},
                        {14, 15},
                        {16, 17}
                    },
                    {
                        {18, 19},
                        {20, 21},
                        {22, 23}
                    }
                });

            // contract a 3*2*2 with a 4*3*2 tensor, summing on (3*2)*2 and 4*(3*2) to produce a 2*4 tensor
            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {110, 290, 470, 650},
                    {125, 341, 557, 773},
                });
            var actual = Tensor.Contract(left, right, new[] { 0, 1 }, new[] { 1, 2 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            // contract a 3*2*2 with a 4*3*2 tensor, summing on (3)*2*(2) and 4*(3*2) to produce a 2*4 tensor
            expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {101, 263, 425, 587},
                    {131, 365, 599, 833},
                });
            actual = Tensor.Contract(left, right, new[] { 0, 2 }, new[] { 1, 2 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }


        [Theory]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void ContractWithSingleLengthDimension(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {1, 2, 3},
                    {4, 5, 6},
                });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 1, 2 },
                    { 3, 4 },
                    { 5, 6 }
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 22, 28 },
                    { 49, 64 }
                });

            // contract a 2*3 with a 3*2 tensor, summing on 2*(3) and (3)*2 to produce a 2*2 tensor
            var actual = Tensor.Contract(left, right, new[] { 1 }, new[] { 0 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));

            // contract a 1*2*3*1 with a 3*2 tensor, summing on 1*2*(3)*1 and (3)*2 to produce a 1*2*1*2 tensor
            var reshapedLeft = left.Reshape(new int[] { 1, 2, 3, 1 });
            var reshapedExpected = expected.Reshape(new int[] { 1, 2, 1, 2 });
            actual = Tensor.Contract(reshapedLeft, right, new[] { 2 }, new[] { 0 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, reshapedExpected));

        }

        [Theory]
        [MemberData(nameof(GetDualTensorConstructors))]
        public void ContractMismatchedDimensions(TensorConstructor leftConstructor, TensorConstructor rightConstructor)
        {
            var left = leftConstructor.CreateFromArray<int>(
                new[] { 0, 1, 2, 3 });

            var right = rightConstructor.CreateFromArray<int>(
                new[,]
                {
                    { 0 },
                    { 1 },
                    { 2 }
                });

            var expected = leftConstructor.CreateFromArray<int>(
                new[,]
                {
                    {0,0,0},
                    {0,1,2},
                    {0,2,4},
                    {0,3,6},
                });

            Assert.Throws<ArgumentException>(() => Tensor.Contract(left, right, new int[] { }, new[] { 1 }));

            // reshape to include dimension of length 1.
            var leftReshaped = left.Reshape(1, (int)left.Length);

            var actual = Tensor.Contract(leftReshaped, right, new[] { 0 }, new[] { 1 });
            Assert.Equal(true, StructuralComparisons.StructuralEqualityComparer.Equals(actual, expected));
        }

        [Theory]
        [MemberData(nameof(GetSingleTensorConstructors))]
        public void GetArrayString(TensorConstructor constructor)
        {
            var tensor = constructor.CreateFromArray<int>(
                new[, ,]
                {
                    {
                        {0, 1},
                        {2, 3},
                        {4, 5}
                    },
                    {
                        {6, 7},
                        {8, 9},
                        {10, 11}
                    },
                    {
                        {12, 13},
                        {14, 15},
                        {16, 17}
                    },
                    {
                        {18, 19},
                        {20, 21},
                        {22, 23}
                    }
                });

            var expected =
@"{
    {
        {0,1},
        {2,3},
        {4,5}
    },
    {
        {6,7},
        {8,9},
        {10,11}
    },
    {
        {12,13},
        {14,15},
        {16,17}
    },
    {
        {18,19},
        {20,21},
        {22,23}
    }
}";

            Assert.Equal(expected, tensor.GetArrayString());

            var expectedNoSpace = expected.Replace(Environment.NewLine, "").Replace(" ", "");
            Assert.Equal(expectedNoSpace, tensor.GetArrayString(false));
        }
    }
}
