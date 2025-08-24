namespace WorkoutAPI.Domain.Enums; public enum Role { Admin = 0, Trainer = 1, Member = 2 }


public enum AdminRole {
    Admin = 1,
    SuperAdmin = 2
}



public enum PaymentMethod {
    Cash = 1,
    CreditCard = 2,
    BankTransfer = 3,
    OnlinePayment = 4
}

public enum PaymentStatus {
    Pending = 1,
    Completed = 2,
    Failed = 3,
    Cancelled = 4,
    Refunded = 5
}

public enum InvoiceStatus {
    Draft = 1,
    Sent = 2,
    Paid = 3,
    Overdue = 4,
    Cancelled = 5
}

public enum ActivityType {
    GeneralWorkout = 1,
    CardioSession = 2,
    WeightTraining = 3,
    GroupClass = 4,
    PersonalTraining = 5
}

public enum BookingStatus {
    Confirmed = 1,
    Cancelled = 2,
    Completed = 3,
    NoShow = 4
}
