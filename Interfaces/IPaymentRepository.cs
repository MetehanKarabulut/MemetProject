using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Models;

namespace MemetProject.Interfaces
{
    public interface IPaymentRepository
    {
        bool Save();
        bool CreatePayment(Payment payment);
        bool PaymentDelete(int paymentId);
        bool PaymentExists(int PaymentId);
        Payment GetPaymentById(int PaymentId);
        ICollection<Payment> GetAllPayments();
        bool PaymentUpdate(Payment payment);
    }
}