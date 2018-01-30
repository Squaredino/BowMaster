//using System;
//using PrefsEditor;
using UnityEngine;
//using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.

// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
public class IapsManager : MonoBehaviour {
//public class IapsManager : MonoBehaviour, IStoreListener {
//    private static IStoreController _mStoreController;          // The Unity Purchasing system.
//    private static IExtensionProvider _mStoreExtensionProvider; // The store-specific Purchasing subsystems.
//    
//    // Product identifiers for all products capable of being purchased: 
//    // "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
//    // counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
//    // also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)
//
//    // General product identifiers for the consumable, non-consumable, and subscription products.
//    // Use these handles in the code to reference which product to purchase. Also use these values 
//    // when defining the Product Identifiers on the store. Except, for illustration purposes, the 
//    // kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
//    // specific mapping to Unity Purchasing's AddProduct, below.
//    private const string iapTier1 =            "com.squaredino.tier1";   
//    private const string iapTier2 =            "com.squaredino.tier2";    
//    private const string iapTierNoAds =        "com.squaredino.noads";  
//    private const string iapTierUnlockAll =    "com.squaredino.unlockall";  
//    private const string iapTierSkin1 =        "com.squaredino.skin1";  
//    private const string iapTierSkin2 =        "com.squaredino.skin2";  
//    private const string iapTierSkin3 =        "com.squaredino.skin3";  
//    private const string iapTierSkin4 =        "com.squaredino.skin4";  
////    private const string KProductIdSubscription = "subscription";
//
//    // Apple App Store-specific product identifier for the subscription product.
////    private const string KProductNameAppleSubscription = "com.unity3d.subscription.new";
//
//    // Google Play Store-specific product identifier subscription product.
////    private const string KProductNameGooglePlaySubscription =  "com.unity3d.subscription.original";
//    
//    
//    public const int IAP_NO_ADS = 0;
//    public const int IAP_COINS_1 = 1;
//    public const int IAP_COINS_2 = 2;
//    public const int IAP_SKIN_1 = 3;
//    public const int IAP_SKIN_2 = 4;
//    public const int IAP_SKIN_3 = 5;
//    public const int IAP_SKIN_4 = 6;
//
//    private void Start()
//    {
//        // If we haven't set up the Unity Purchasing reference
//        if (_mStoreController == null)
//        {
//            // Begin to configure our connection to Purchasing
//            InitializePurchasing();
//        }
//    }
//    
//    private void OnEnable()
//    {
//        GlobalEvents<OnIAPsBuySkin>.Happened += OnIAPsBuySkin; 
//        GlobalEvents<OnIAPsBuyTier1>.Happened += OnIAPsBuyTier1; 
//        GlobalEvents<OnIAPsBuyTier2>.Happened += OnIAPsBuyTier2; 
//        GlobalEvents<OnIAPsBuyNoAds>.Happened += OnIAPsBuyNoAds; 
//        GlobalEvents<OnIAPsBuyUnlockAll>.Happened += OnIAPsBuyUnlockAll; 
//    }
//    
//    private void OnIAPsBuySkin(OnIAPsBuySkin obj)
//    {
//        BuySkin(obj.Id);
//    }
//    
//    private void OnIAPsBuyTier1(OnIAPsBuyTier1 obj)
//    {
//        BuyTier1();
//    }
//    
//    private void OnIAPsBuyTier2(OnIAPsBuyTier2 obj)
//    {
//        BuyTier2();
//    }
//    
//    private void OnIAPsBuyNoAds(OnIAPsBuyNoAds obj)
//    {
//        BuyNoAds();
//    }
//    
//    private void OnIAPsBuyUnlockAll(OnIAPsBuyUnlockAll obj)
//    {
//        BuyTierUnlockAll();
//    }
//    
//    private void BuySkin(int id)
//    {
//        BuyProductId(iapTier1);
//        GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = id});
//        PrefsManager.IsFirstBuy = true;
//        SecurePlayerPrefs.GetBool("IsFirstBuy", true);
//    }
//    
//    public void BuyTier1()
//    {
//        BuyProductId(iapTier2);
//    }
//
//    public void BuyTier2()
//    {
//        BuyProductId(iapTier2);
//    }
//    
//    public void BuyTierUnlockAll()
//    {
//        BuyProductId(iapTierUnlockAll);
//    }
//
//    public void BuyNoAds()
//    {
//        BuyProductId(iapTierNoAds);
//    }
//
//    private void InitializePurchasing() 
//    {
//        // If we have already connected to Purchasing ...
//        if (IsInitialized())
//        {
//            // ... we are done here.
//            return;
//        }
//        
//        // Create a builder, first passing in a suite of Unity provided stores.
//        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
//        
//        // Add a product to sell / restore by way of its identifier, associating the general identifier
//        // with its store-specific identifiers.
//        builder.AddProduct(iapTier1, ProductType.Consumable);
//        builder.AddProduct(iapTier2, ProductType.Consumable);
//        // Continue adding the non-consumable product.
//        builder.AddProduct(iapTierUnlockAll, ProductType.NonConsumable);
//        builder.AddProduct(iapTierNoAds, ProductType.NonConsumable);
//        builder.AddProduct(iapTierSkin1, ProductType.NonConsumable);
//        builder.AddProduct(iapTierSkin2, ProductType.NonConsumable);
//        builder.AddProduct(iapTierSkin3, ProductType.NonConsumable);
//        builder.AddProduct(iapTierSkin4, ProductType.NonConsumable);
//        // And finish adding the subscription product. Notice this uses store-specific IDs, illustrating
//        // if the Product ID was configured differently between Apple and Google stores. Also note that
//        // one uses the general kProductIDSubscription handle inside the game - the store-specific IDs 
//        // must only be referenced here. 
////        builder.AddProduct(KProductIdSubscription, ProductType.Subscription, new IDs(){
////            { KProductNameAppleSubscription, AppleAppStore.Name },
////            { KProductNameGooglePlaySubscription, GooglePlay.Name },
////        });
//        
//        // Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
//        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
//        UnityPurchasing.Initialize(this, builder);
//    }
//    
//    
//    private bool IsInitialized()
//    {
//        // Only say we are initialized if both the Purchasing references are set.
//        return _mStoreController != null && _mStoreExtensionProvider != null;
//    }
//
//
//    private void BuyProductId(string productId)
//    {
//        // If Purchasing has been initialized ...
//        if (IsInitialized())
//        {
//            // ... look up the Product reference with the general product identifier and the Purchasing 
//            // system's products collection.
//            Product product = _mStoreController.products.WithID(productId);
//            
//            // If the look up found a product for this device's store and that product is ready to be sold ... 
//            if (product != null && product.availableToPurchase)
//            {
//                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
//                // ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
//                // asynchronously.
//                _mStoreController.InitiatePurchase(product);
//            }
//            // Otherwise ...
//            else
//            {
//                // ... report the product look-up failure situation  
//                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
//            }
//        }
//        // Otherwise ...
//        else
//        {
//            // ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
//            // retrying initiailization.
//            Debug.Log("BuyProductID FAIL. Not initialized.");
//        }
//    }
//    
//    
//    // Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
//    // Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
//    public void RestorePurchases()
//    {
//        // If Purchasing has not yet been set up ...
//        if (!IsInitialized())
//        {
//            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
//            Debug.Log("RestorePurchases FAIL. Not initialized.");
//            return;
//        }
//        
//        // If we are running on an Apple device ... 
//        if (Application.platform == RuntimePlatform.IPhonePlayer || 
//            Application.platform == RuntimePlatform.OSXPlayer)
//        {
//            // ... begin restoring purchases
//            Debug.Log("RestorePurchases started ...");
//            
//            // Fetch the Apple store-specific subsystem.
//            var apple = _mStoreExtensionProvider.GetExtension<IAppleExtensions>();
//            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
//            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
//            apple.RestoreTransactions((result) => {
//                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
//                // no purchases are available to be restored.
//                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
//            });
//        }
//        // Otherwise ...
//        else
//        {
//            // We are not running on an Apple device. No work is necessary to restore purchases.
//            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
//        }
//    }
//    
//    
//    //  
//    // --- IStoreListener
//    //
//    
//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
//    {
//        // Purchasing has succeeded initializing. Collect our Purchasing references.
//        Debug.Log("OnInitialized: PASS");
//        
//        // Overall Purchasing system, configured with products for this application.
//        _mStoreController = controller;
//        // Store specific subsystem, for accessing device-specific store features.
//        _mStoreExtensionProvider = extensions;
//    }
//    
//    
//    public void OnInitializeFailed(InitializationFailureReason error)
//    {
//        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
//        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
//    }
//    
//    
//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) 
//    {
//        // A consumable product has been purchased by this user.
//        if (String.Equals(args.purchasedProduct.definition.id, iapTier1, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
//            GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 200});
//            PrefsManager.IsFirstBuy = true;
//            SecurePlayerPrefs.GetBool("IsFirstBuy", true);
//        }// A consumable product has been purchased by this user.
//        if (String.Equals(args.purchasedProduct.definition.id, iapTier2, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//            // The consumable item has been successfully purchased, add 100 coins to the player's in-game score.
//            GlobalEvents<OnCoinsAdd>.Call(new OnCoinsAdd {Count = 1000});
//            PrefsManager.IsFirstBuy = true;
//            SecurePlayerPrefs.GetBool("IsFirstBuy", true);
//        }
//        // Or ... a non-consumable product has been purchased by this user.
//        else if (String.Equals(args.purchasedProduct.definition.id, iapTierUnlockAll, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//            
//            GlobalEvents<OnAdsDisable>.Call(new OnAdsDisable ());
//            GlobalEvents<OnSkinsUnlockAll>.Call(new OnSkinsUnlockAll ());
//            PrefsManager.IsFirstBuy = true;
//            SecurePlayerPrefs.SetBool("IsFirstBuy", true);
//            SecurePlayerPrefs.SetBool("IsAllUnlocked", true);
//            GlobalEvents<OnScreenCoinsHide>.Call(new OnScreenCoinsHide ());
//        }// Or ... a non-consumable product has been purchased by this user.
//        else if (String.Equals(args.purchasedProduct.definition.id, iapTierNoAds, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//           
//            MyAds.noAds = 1;
//            SecurePlayerPrefs.SetInt ("noAds", MyAds.noAds);
//        }else if (String.Equals(args.purchasedProduct.definition.id, iapTierSkin1, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//           
//            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_1});
//        }else if (String.Equals(args.purchasedProduct.definition.id, iapTierSkin2, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//           
//            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_2});
//        }else if (String.Equals(args.purchasedProduct.definition.id, iapTierSkin3, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//           
//            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_3});
//        }else if (String.Equals(args.purchasedProduct.definition.id, iapTierSkin4, StringComparison.Ordinal))
//        {
//            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
//           
//            GlobalEvents<OnBuySkinByIAP>.Call(new OnBuySkinByIAP{Id = IAP_SKIN_4});
//        }
//        // Or ... an unknown product has been purchased by this user. Fill in additional products here....
//        else 
//        {
//            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
//        }
//
//        // Return a flag indicating whether this product has completely been received, or if the application needs 
//        // to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
//        // saving purchased products to the cloud, and when that save is delayed. 
//        return PurchaseProcessingResult.Complete;
//    }
//    
//    
//    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
//    {
//        // A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
//        // this reason with the user to guide their troubleshooting actions.
//        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
//    }
}
