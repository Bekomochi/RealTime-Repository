<?php

use App\Http\Controllers\AccountController;
use App\Http\Controllers\AdminController;
use App\Http\Controllers\AuthController;
use App\Http\Middleware\AuthMiddleware;
use Illuminate\Support\Facades\Route;

/* Route::post('○○/□□'～);の['○○/□□']はurl
 * formのurl関数に記入する */

//ルーティング
//通常時表示画面
Route::get('/', [AuthController::class, 'login']);

//ログイン画面、ログイン処理
Route::get('auth/login', [AuthController::class, 'login']);//ログイン画面
Route::post('auth/dologin', [AuthController::class, 'dologin']);//ログイン処理

//ログアウト処理
Route::get('auth/logout', [AuthController::class, 'logout']);//ログアウト
Route::post('auth/dologout', [AuthController::class, 'dologout']);//ログアウト処理

/*ルートのグループ化
[accounts]に集約した為、ルートで['accounts/○○']とする必要は無い
nameで['accounts.index']と結合される
*/
Route::prefix('accounts')->name('accounts.')->controller(AccountController::class)
    ->middleware(AuthMiddleware::class)->group(function () {
        //一覧表示画面
        Route::get('/', 'index')->name('index');

        //新規登録画面、登録処理
        Route::get('create', 'create')->name('create');//登録画面
        Route::get('create_cmp', 'create_cmp')->name('create_cmp');//登録完了画面
        Route::post('store', 'store')->name('store');//登録処理

        //削除確認画面、削除処理
        Route::get('delete', 'delete')->name('delete');//削除確認画面
        Route::get('delete_cmp', 'delete_cmp')->name('delete_cmp');//削除完了画面
        Route::post('destroy', 'destroy')->name('destroy');//削除処理
    });

//個別記事

Route::get('admin/item', [AdminController::class, 'item']);//アイテム
Route::get('admin/accounts', [AdminController::class, 'accounts']);//ユーザー
Route::get('admin/users', [AdminController::class, 'users']);//ユーザー
Route::get('admin/having', [AdminController::class, 'having']);//アイテム所持
