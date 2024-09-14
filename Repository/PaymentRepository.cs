using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemetProject.Data;
using MemetProject.Interfaces;
using MemetProject.Models;

namespace MemetProject.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly DataContext _context;
        public PaymentRepository(DataContext context)
        {
            _context = context;
        }
        public bool CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            return Save();
        }

        public ICollection<Payment> GetAllPayments()
        {
            return _context.Payments.OrderBy(p => p.PaymentId).ToList();
        }

        public Payment GetPaymentById(int PaymentId)
        {
            return _context.Payments.Where(p => p.PaymentId == PaymentId).SingleOrDefault();
        }

        public bool PaymentDelete(int paymentId)
        {
            var payment = _context.Payments.Where(p => p.PaymentId == paymentId).SingleOrDefault();

            if(payment == null)
                return false;

            _context.Payments.Remove(payment);

            return Save();
        }

        public bool PaymentExists(int PaymentId)
        {
            return _context.Payments.Any(p => p.PaymentId == PaymentId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool PaymentUpdate(Payment Payment)
        {
            var existingPayment = _context.Payments.FirstOrDefault(u => u.PaymentId == Payment.PaymentId);

            if (!PaymentExists(Payment.PaymentId))
                return false;


            _context.Entry(existingPayment).CurrentValues.SetValues(Payment);

            _context.Payments.Update(existingPayment);
            return Save();
        }
    }
}