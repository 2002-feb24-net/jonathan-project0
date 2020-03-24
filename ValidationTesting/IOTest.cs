using System;
using Xunit;

using WheyMenIOValidation.Library;

namespace ValidationTesting
{
    public class IOTest
    {

        [Theory]
        [InlineData("jon")]
        [InlineData("o'connor")]
        [InlineData("jon-c")]
        [InlineData("документи")]
        [InlineData("映月")]
        public void TestNameValidation(string value)
        {
            Assert.True(CustomerInfoValidation.ValidateName(value));
        }
        [Theory]
        [InlineData("")]
        public void TestNameInvalidation(string value)
        {
            Assert.False(CustomerInfoValidation.ValidateName(value));
        }
        [Theory]
        [InlineData("jhbui1@uci.edu")]
        [InlineData("jhbui1@uci.co.uk")]
        public void TestEmailValidation(string value)
        {
            Assert.True(CustomerInfoValidation.ValidateEmail(value));
        }
        [Theory]
        [InlineData("jhbui1@uci")]
        [InlineData("@.com")]
        [InlineData("")]
        [InlineData("@")]
        [InlineData("abc")]
        [InlineData("abc@")]
        public void TestEmailInvalidation(string value)
        {
            Assert.False(CustomerInfoValidation.ValidateEmail(value));
        }
       
        [Fact]
        public void TestUsernameValidation()
        {
            Assert.True(CustomerInfoValidation.ValidateUsername("jhbui1"));
        }
        [Fact]
        public void TestUsernameInvalidation()
        {
            Assert.False(CustomerInfoValidation.ValidateUsername("ab"));
        }
        [Fact]
        public void TestPwdValidation()
        {
            Assert.True(CustomerInfoValidation.ValidatePwd("jhbui1&@#$"));
        }
        [Fact]
        public void TestPwdInvalidation()
        {
            Assert.False(CustomerInfoValidation.ValidatePwd("&6"));
        }

    }
}
