using UnityEngine;
//using VoxelBusters.NativePlugins;


public class BillingManager : MonoBehaviour
{
    public const int iapTier1 = 0;
    public const int iapTier2 = 1;
    public const int iapTierNoAds = 2;
    public const int iapTierUnlockAll = 3;
    public const int iapTierSkin1 = 4;
    public const int iapTierSkin2 = 5;
    public const int iapTierSkin3 = 6;
    public const int iapTierSkin4 = 7;
    
    private void Start()
    {
//        if (IsAvailable () && CanMakePayments ())
            RequestBillingProducts ();
    }

    private void OnEnable()
    {
//        Billing.DidFinishRequestForBillingProductsEvent	+= OnDidFinishProductsRequest;
//        Billing.DidFinishProductPurchaseEvent	        += OnDidFinishTransaction;
//        Billing.DidFinishRestoringPurchasesEvent		+= OnDidFinishRestoringPurchases;
        GlobalEvents<OnIAPsBuySkin>.Happened += OnIAPsBuySkin; 
    }

    private void OnIAPsBuySkin(OnIAPsBuySkin obj)
    {
        BuySkin(obj.Id);
    }

//    private bool IsAvailable ()
//    {
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "BillingIsAvailable " + NPBinding.Billing.IsAvailable()});
//        return NPBinding.Billing.IsAvailable();
//    }
//
//    private bool CanMakePayments ()
//    {
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "BillingCanMakePayments " + NPBinding.Billing.CanMakePayments()});
//        return NPBinding.Billing.CanMakePayments();
//    }

    public void RequestBillingProducts ()
    {
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "RequestBillingProducts()"});
//        NPBinding.Billing.RequestForBillingProducts(NPSettings.Billing.Products);
//        // At this point you can display an activity indicator to inform user that task is in progress
    }

//    private void OnDidFinishProductsRequest (BillingProduct[] _regProductsList, string _error)
//    {
//        // Hide activity indicator
//        
//        // Handle response
//        if (_error != null)
//        {        
//            // Something went wrong
//            GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "RequestBillingProducts() - ERROR " + _error});
//        }
//        else 
//        {  
//            GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "RequestBillingProducts() - Count = " + _regProductsList.Length});
//            // Inject code to display received products
//            foreach (BillingProduct _product in _regProductsList) {
//                GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "RequestBillingProducts() - Product Identifier = " + _product.ProductIdentifier});
//                Debug.Log("Product Identifier = "         + _product.ProductIdentifier);
//                Debug.Log("Product Description = "        + _product.Description);
//            }
//        }
//    }
//
//    private void BuyItem (BillingProduct _product)
//    {
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "BuyItem() - Product Identifier = " + _product.ProductIdentifier});
//        if (NPBinding.Billing.IsProductPurchased(_product))
//        {
//            // Show alert message that item is already purchased
//            GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "BuyItem() - Almoust Purshased - Product Identifier = " + _product.ProductIdentifier});
//            return;
//        }
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "BuyItem() - Try Purchase - Product Identifier = " + _product.ProductIdentifier});
//        // Call method to make purchase
//        NPBinding.Billing.BuyProduct(_product);
//
//        // At this point you can display an activity indicator to inform user that task is in progress
//    }

    public void BuyTier1()
    {
        // Buy the consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
//        BuyItem(NPSettings.Billing.Products[iapTier1]);
    }

    public void BuyTier2()
    {
//        BuyItem(NPSettings.Billing.Products[iapTier2]);
    }
    
    public void BuyTierUnlockAll()
    {
//        BuyItem(NPSettings.Billing.Products[iapTierUnlockAll]);
    }

    public void BuySkin(int id)
    {
//        BuyItem(NPSettings.Billing.Products[id]);
    }

    public void BuyNoAds()
    {
        // Buy the non-consumable product using its general identifier. Expect a response either 
        // through ProcessPurchase or OnPurchaseFailed asynchronously.
//        BuyItem(NPSettings.Billing.Products[iapTierNoAds]);
    }

//    private void OnDidFinishTransaction (BillingTransaction _transaction)
//    {
//        Debug.Log ("OnDidFinishTransaction()");
//        if (_transaction != null)
//        {
//            if (_transaction.VerificationState == eBillingTransactionVerificationState.SUCCESS)
//            {
//                if (_transaction.TransactionState == eBillingTransactionState.PURCHASED)
//                {
//                    GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "OnDidFinishTransaction() - Product Identifier = " + _transaction.ProductIdentifier});
//                    // Your code to handle purchased products
//                    if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTier1].ProductIdentifier) {
//                        GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
//                        PrefsManager.IsFirstBuy = true;
//                        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
//                        D.Log ("OnDidFinishTransaction() - 200 coins (bought)");
//                    } else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTier2].ProductIdentifier) {
//                            GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1000});
//                        PrefsManager.IsFirstBuy = true;
//                        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
//                            D.Log ("OnDidFinishTransaction() - 1000 coins (bought)");
//                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTierUnlockAll].ProductIdentifier) {
//                        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
//                        GlobalEvents<OnSkinsUnlockAll>.Call(new OnSkinsUnlockAll ());
//                        PrefsManager.IsFirstBuy = true;
//                        SecurePlayerPrefs.SetBool("IsFirstBuy", true);
//                        SecurePlayerPrefs.SetBool("IsAllUnlocked", true);
//                        GlobalEvents<OnScreenCoinsHide>.Call(new OnScreenCoinsHide ());
//                        D.Log ("OnDidFinishTransaction() - NoAds (bought)");
//                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin1].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin1});
//                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin2].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin2});
//                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin3].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin3});
//                    }else if (_transaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin4].ProductIdentifier)
//                    {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin4});
//                    }
//
//                    return;
//                } else {
//                    NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "", "Ok", _buttonPressed => {});
//                    GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "Purchase failed"});
//                }
//            }
//            NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "", "Ok", _buttonPressed => {});
//            GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "Purchase failed"});
//            return;
//        }
//
//        NPBinding.UI.ShowAlertDialogWithSingleButton("Purchase failed", "Check your Internet connection or try later!", "Ok", _buttonPressed => {});
//        GlobalEvents<OnDebugLog>.Call(new OnDebugLog {message = "Purchase failed-Check your Internet connection or try later!"});
//    }

    public void BtnRestoreIaps()
    {
//        Debug.Log("BtnRestoreIaps()");
//        NPBinding.Billing.RestorePurchases ();
    }

//    private void OnDidFinishRestoringPurchases (BillingTransaction[] _transactions, string _error)
//    {
//        Debug.Log(string.Format("Received restore purchases response. Error = " + _error));
//
//        if (_transactions != null)
//        {                
//            Debug.Log("Count of transaction information received = "+_transactions.Length);
//
//            foreach (BillingTransaction currentTransaction in _transactions)
//            {
//
//                if (currentTransaction.TransactionState == eBillingTransactionState.RESTORED) {
//                    if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierNoAds].ProductIdentifier) {
//                        MyAds.noAds = 1;
//                        SecurePlayerPrefs.SetInt ("noAds", MyAds.noAds);
//                    } else if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierUnlockAll].ProductIdentifier) {
//                        GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
//                        GlobalEvents<OnSkinsUnlockAll>.Call(new OnSkinsUnlockAll ());
//                        PrefsManager.IsFirstBuy = true;
//                        SecurePlayerPrefs.SetBool("IsFirstBuy", true);
//                        SecurePlayerPrefs.SetBool("IsAllUnlocked", true);
//                        GlobalEvents<OnScreenCoinsHide>.Call(new OnScreenCoinsHide ());
//                    } else if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin1].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin1});
//                    } else  if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin2].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin2});
//                    } else  if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin3].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin3});
//                    } else  if (currentTransaction.ProductIdentifier == NPSettings.Billing.Products[iapTierSkin4].ProductIdentifier) {
//                        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = iapTierSkin4});
//                    }  
//                }
//                Debug.Log("Product Identifier = "         + currentTransaction.ProductIdentifier);
//                Debug.Log("Transaction State = "        + currentTransaction.TransactionState);
//                Debug.Log("Verification State = "        + currentTransaction.VerificationState);
//                Debug.Log("Transaction Date[UTC] = "    + currentTransaction.TransactionDateUTC);
//                Debug.Log("Transaction Date[Local] = "    + currentTransaction.TransactionDateLocal);
//                Debug.Log("Transaction Identifier = "    + currentTransaction.TransactionIdentifier);
//                Debug.Log("Transaction Receipt = "        + currentTransaction.TransactionReceipt);
//                Debug.Log("Error = "                    + currentTransaction.Error);
//            }
//
//            return;
//        }
//
//        NPBinding.UI.ShowAlertDialogWithSingleButton("Restore purchase failed", "", "Ok", _buttonPressed => {});
//    }
}