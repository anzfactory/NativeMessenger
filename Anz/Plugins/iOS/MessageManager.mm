#import <Foundation/Foundation.h>

@interface AlertManager : NSObject
@property (nonatomic) NSString *callbackObject;
@property (nonatomic) NSString *callbackMethod;
- (void)showAlertWithMessage:(NSString *)message;
@end

@implementation AlertManager

- (void)showAlertWithMessage:(NSString *)message {
    UIAlertController *alertController = [UIAlertController alertControllerWithTitle:nil message:message preferredStyle:UIAlertControllerStyleAlert];
    [alertController addAction:[UIAlertAction actionWithTitle:@"OK" style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
        UnitySendMessage([self.callbackObject UTF8String], [self.callbackMethod UTF8String], "");
    }]];
    UIViewController *controller = UnityGetGLViewController();
    [controller presentViewController:alertController animated:YES completion:nil];
}

@end


extern "C" {

	void showToast(const char* message) {
		NSString *msg = [NSString stringWithUTF8String:message];

        UIView *view = UnityGetGLView();

        UILabel *label = [[UILabel alloc] initWithFrame:CGRectMake(10.f, 10.f, view.frame.size.width - 20.f, 0)];
        label.textAlignment = NSTextAlignmentCenter;
        label.text = msg;
        label.numberOfLines = 0;
        label.font = [UIFont systemFontOfSize:12.f];
        label.textColor = [UIColor whiteColor];
        CGRect r = label.frame;
        [label sizeToFit];
        label.frame = CGRectMake(r.origin.x, r.origin.y, r.size.width, label.frame.size.height);

        CGFloat h = label.frame.size.height + 20.f;
        UIView *toast = [[UIView alloc] initWithFrame:CGRectMake(0, view.frame.size.height - h, view.frame.size.width, h)];
        toast.backgroundColor = [UIColor colorWithRed:0 green:0 blue:0 alpha:0.8f];
        [toast addSubview:label];
        [view addSubview:toast];

        [UIView animateWithDuration:0.5f delay:2.0f options:UIViewAnimationOptionCurveEaseInOut animations:^{
            toast.frame = CGRectMake(0, view.frame.size.height, view.frame.size.width, h);
        } completion:^(BOOL finished) {
            [label removeFromSuperview];
        }];
    }

	void showAlert(const char* message, const char* callbackObjectName, const char* callbackMethodName) {
        NSString *msg = [NSString stringWithUTF8String:message];
        AlertManager *alert = [[AlertManager alloc] init];
        alert.callbackObject = [NSString stringWithUTF8String:callbackObjectName];
        alert.callbackMethod = [NSString stringWithUTF8String:callbackMethodName];
        [alert showAlertWithMessage:msg];
    }
}
