<?php

namespace App\Http\Controllers;

use App\Models\Account;
use App\Models\Item;
use Illuminate\Http\Request;

class AdminController extends Controller
{
    public function item(Request $request)
    {
        //ログアウト状態だったらログイン画面へ遷移
        if (!$request->session()->exists('login')) {
            return redirect('auth/login');
        }
        $items = Item::all();
        return view('Admin.itemView', ['items' => $items]);//アイテム一覧を表示
    }

    public function accounts(Request $request)
    {
        //ログアウト状態だったらログイン画面へ遷移
        if (!$request->session()->exists('login')) {
            return redirect('auth/login');
        }
        $accounts = Account::all();
        //dd($accounts);
        return view('Admin.accountsView', ['accounts' => $accounts]);
    }

    public function having(Request $request)
    {
        //ログアウト状態だったらログイン画面へ遷移
        if (!$request->session()->exists('login')) {
            return redirect('auth/login');
        }

        return view('Admin/havingView');//所持プレイヤーを表示
    }
}
